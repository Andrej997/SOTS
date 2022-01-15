using API.Application.Common.Interfaces;
using API.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace API.Application.Tests.Queries.ExportQTI
{
    public class ExportQTIQuery : IRequest<byte[]>
    {
        public long TestId { get; set; }
    }

    public class GetEdgesQuerryHandler : IRequestHandler<ExportQTIQuery, byte[]>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public GetEdgesQuerryHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<byte[]> Handle(ExportQTIQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var test = _context.Tests
                    .Where(test => test.Id == request.TestId)
                    .Include(test => test.Questions)
                        .ThenInclude(question => question.Answers)
                    .FirstOrDefault();

                XmlDocument xmlDoc = new XmlDocument();

                XmlNode xmldocNode = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                xmlDoc.AppendChild(xmldocNode);

                XmlElement rootNode = xmlDoc.CreateElement("qti-assessment-test", "http://www.imsglobal.org/xsd/imsqti_v3p0");
                rootNode.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                rootNode.SetAttribute("identifier", test.Id.ToString());
                rootNode.SetAttribute("title", test.Name);
                rootNode.SetAttribute("xsi:schemaLocation", "http://www.imsglobal.org/xsd/imsqti_v3p0 https://purl.imsglobal.org/spec/qti/v3p0/xsd/imsqti_asiv3p0_v1p0.xsd");
                rootNode.SetAttribute("xml:lang", "en-US");
                xmlDoc.AppendChild(rootNode);

                XmlElement qtiTestPart = xmlDoc.CreateElement("qti-test-part");
                qtiTestPart.SetAttribute("identifier", Guid.NewGuid().ToString());
                qtiTestPart.SetAttribute("navigation-mode", "nonlinear");
                qtiTestPart.SetAttribute("submission-mode", "individual");
                rootNode.AppendChild(qtiTestPart);

                XmlElement qtiAssessmentSection = xmlDoc.CreateElement("qti-assessment-section");
                qtiAssessmentSection.SetAttribute("identifier", Guid.NewGuid().ToString());
                qtiAssessmentSection.SetAttribute("title", "Section 1");
                qtiAssessmentSection.SetAttribute("visible", "true");
                qtiTestPart.AppendChild(qtiAssessmentSection);

                var qtiQuestions = new List<XmlDocument>();

                foreach (var question in test.Questions)
                {
                    XmlElement qtiAssessmentItemRef = xmlDoc.CreateElement("qti-assessment-item-ref");
                    qtiAssessmentItemRef.SetAttribute("identifier", question.Id.ToString());
                    qtiAssessmentItemRef.SetAttribute("href", $"question_{question.Id}");
                    qtiTestPart.AppendChild(qtiAssessmentItemRef);

                    qtiQuestions.Add(CreateQuestionIQT(question));
                }

                byte[] testBytes = Encoding.Default.GetBytes(xmlDoc.OuterXml);

                byte[] archiveFile;
                using (var archiveStream = new MemoryStream())
                {
                    using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
                    {
                        var zipArchiveEntry = archive.CreateEntry($"test_{test.Id}.xml", CompressionLevel.Fastest);
                        using (var zipStream = zipArchiveEntry.Open())
                            zipStream.Write(testBytes, 0, testBytes.Length);

                        for (int i = 0; i < test.Questions.Count; i++)
                        {
                            byte[] qustionBytes = Encoding.Default.GetBytes(qtiQuestions[i].OuterXml);
                            var zipArchiveEntryQuestion = archive.CreateEntry($"question_{test.Questions[i].Id}.xml", CompressionLevel.Fastest);
                            using (var zipStream = zipArchiveEntryQuestion.Open())
                                zipStream.Write(qustionBytes, 0, qustionBytes.Length);
                        }
                    }

                    archiveFile = archiveStream.ToArray();
                }

                //xmlDoc.Save("test.xml");

                return archiveFile;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private XmlDocument CreateQuestionIQT(Question question)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode xmldocNode = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDoc.AppendChild(xmldocNode);

            XmlElement rootNode = xmlDoc.CreateElement("qti-assessment-item", "http://www.imsglobal.org/xsd/qti/imsqtiasi_v3p0");
            rootNode.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            rootNode.SetAttribute("identifier", question.Id.ToString());
            rootNode.SetAttribute("time-dependent", "false");
            rootNode.SetAttribute("xsi:schemaLocation", "http://www.imsglobal.org/xsd/imsqti_v3p0 https://purl.imsglobal.org/spec/qti/v3p0/xsd/imsqti_asiv3p0_v1p0.xsd");
            rootNode.SetAttribute("xml:lang", "en-US");
            xmlDoc.AppendChild(rootNode);

            XmlElement qtiResponseDeclaration = xmlDoc.CreateElement("qti-response-declaration");
            qtiResponseDeclaration.SetAttribute("base-type", "identifier");
            if (question.Answers.Where(answer => answer.IsCorrect == true).Count() == 1)
                qtiResponseDeclaration.SetAttribute("cardinality", "single");
            else
                qtiResponseDeclaration.SetAttribute("cardinality", "multiple");
            qtiResponseDeclaration.SetAttribute("identifier", "RESPONSE");
            rootNode.AppendChild(qtiResponseDeclaration);

            XmlElement qtiCorrectResponse = xmlDoc.CreateElement("qti-correct-response");
            qtiResponseDeclaration.AppendChild(qtiCorrectResponse);

            var correctAnswers = question.Answers.Where(answer => answer.IsCorrect == true).ToList();
            for (int i = 0; i < correctAnswers.Count; i++)
            {
                XmlElement qtiValue = xmlDoc.CreateElement("qti-value");
                qtiValue.InnerText = (i + 1).ToString();
                qtiCorrectResponse.AppendChild(qtiValue);
            }

            XmlElement qtiOutcomeDeclaration = xmlDoc.CreateElement("qti-outcome-declaration");
            qtiOutcomeDeclaration.SetAttribute("base-type", "float");
            qtiOutcomeDeclaration.SetAttribute("cardinality", "single");
            qtiOutcomeDeclaration.SetAttribute("identifier", "SCORE");
            rootNode.AppendChild(qtiOutcomeDeclaration);

            XmlElement qtiDefaultValue = xmlDoc.CreateElement("qti-default-value");
            qtiOutcomeDeclaration.AppendChild(qtiDefaultValue);

            XmlElement qtiValue2 = xmlDoc.CreateElement("qti-value");
            qtiValue2.InnerText = question.Points.ToString();
            qtiDefaultValue.AppendChild(qtiValue2);

            XmlElement qtiItemBody = xmlDoc.CreateElement("qti-item-body");
            rootNode.AppendChild(qtiItemBody);

            XmlElement p = xmlDoc.CreateElement("p");
            p.InnerText = question.TextQuestion;
            qtiItemBody.AppendChild(p);

            XmlElement qtiChoiceInteraction = xmlDoc.CreateElement("qti-choice-interaction");
            qtiChoiceInteraction.SetAttribute("max-choices", correctAnswers.Count.ToString());
            qtiChoiceInteraction.SetAttribute("response-identifier", "RESPONSE");
            qtiItemBody.AppendChild(qtiChoiceInteraction);

            for (int i = 0; i < question.Answers.Count; i++)
            {
                XmlElement qtiSimpleChoice = xmlDoc.CreateElement("qti-simple-choice");
                qtiSimpleChoice.SetAttribute("identifier", (i + 1).ToString());
                qtiSimpleChoice.InnerText = question.Answers[i].TextAnswer;
                qtiChoiceInteraction.AppendChild(qtiSimpleChoice);
            }

            XmlElement qtiResponseProcessing = xmlDoc.CreateElement("qti-response-processing");
            qtiResponseProcessing.SetAttribute("template", "https://purl.imsglobal.org/spec/qti/v3p0/rptemplates/match_correct");
            rootNode.AppendChild(qtiResponseProcessing);

            //xmlDoc.Save($"{question.Id}.xml");
            return xmlDoc;
        }
    }
}
