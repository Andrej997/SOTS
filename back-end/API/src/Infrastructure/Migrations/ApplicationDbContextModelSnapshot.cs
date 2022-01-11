﻿// <auto-generated />
using System;
using API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace API.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "C.UTF-8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("API.Domain.Entities.Answer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("boolean")
                        .HasColumnName("is_correct");

                    b.Property<long>("QuestionId")
                        .HasColumnType("bigint")
                        .HasColumnName("question_id");

                    b.Property<string>("TextAnswer")
                        .HasColumnType("text")
                        .HasColumnName("text_answer");

                    b.HasKey("Id")
                        .HasName("pk_answer");

                    b.HasIndex("QuestionId");

                    b.ToTable("answers", "project");
                });

            modelBuilder.Entity("API.Domain.Entities.ChoosenAnswer", b =>
                {
                    b.Property<long>("StudentTestId")
                        .HasColumnType("bigint")
                        .HasColumnName("student_test_id");

                    b.Property<long>("QuestionId")
                        .HasColumnType("bigint")
                        .HasColumnName("question_id");

                    b.Property<long>("AnswerId")
                        .HasColumnType("bigint")
                        .HasColumnName("answer_id");

                    b.Property<DateTime>("AnswerDated")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("answer_dated");

                    b.HasKey("StudentTestId", "QuestionId", "AnswerId")
                        .HasName("pk_choosen_answer");

                    b.ToTable("choosen_answers", "project");
                });

            modelBuilder.Entity("API.Domain.Entities.Domain", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("date_created");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<long>("SubjectId")
                        .HasColumnType("bigint")
                        .HasColumnName("subject_id");

                    b.HasKey("Id")
                        .HasName("pk_domain");

                    b.ToTable("domains", "project");
                });

            modelBuilder.Entity("API.Domain.Entities.Edge", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("date_created");

                    b.Property<long>("DomainId")
                        .HasColumnType("bigint")
                        .HasColumnName("domain_id");

                    b.Property<string>("Label")
                        .HasColumnType("text")
                        .HasColumnName("label");

                    b.Property<string>("SourceId")
                        .HasColumnType("text")
                        .HasColumnName("souce_id");

                    b.Property<string>("TargetId")
                        .HasColumnType("text")
                        .HasColumnName("target_id");

                    b.HasKey("Id")
                        .HasName("pk_edge");

                    b.ToTable("edges", "project");
                });

            modelBuilder.Entity("API.Domain.Entities.Grade", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<double>("FromProcentage")
                        .HasColumnType("double precision")
                        .HasColumnName("from_procentage");

                    b.Property<string>("Label")
                        .HasColumnType("text")
                        .HasColumnName("label");

                    b.Property<double>("ToProcentage")
                        .HasColumnType("double precision")
                        .HasColumnName("to_procentage");

                    b.HasKey("Id")
                        .HasName("pk_grade");

                    b.ToTable("grades", "project");
                });

            modelBuilder.Entity("API.Domain.Entities.Node", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("date_created");

                    b.Property<long>("DomainId")
                        .HasColumnType("bigint")
                        .HasColumnName("domain_id");

                    b.Property<string>("Label")
                        .HasColumnType("text")
                        .HasColumnName("label");

                    b.HasKey("Id")
                        .HasName("pk_node");

                    b.ToTable("nodes", "project");
                });

            modelBuilder.Entity("API.Domain.Entities.Question", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Image")
                        .HasColumnType("text")
                        .HasColumnName("image");

                    b.Property<long>("Points")
                        .HasColumnType("bigint")
                        .HasColumnName("points");

                    b.Property<string>("ProblemNodeId")
                        .HasColumnType("text")
                        .HasColumnName("problem_node_id");

                    b.Property<long>("TestId")
                        .HasColumnType("bigint")
                        .HasColumnName("test_id");

                    b.Property<string>("TextQuestion")
                        .HasColumnType("text")
                        .HasColumnName("text_question");

                    b.HasKey("Id")
                        .HasName("pk_question");

                    b.HasIndex("TestId");

                    b.ToTable("questions", "project");
                });

            modelBuilder.Entity("API.Domain.Entities.QuestionTime", b =>
                {
                    b.Property<DateTime>("QuestionStart")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("question_start");

                    b.Property<long>("StudentTestsId")
                        .HasColumnType("bigint")
                        .HasColumnName("student_tests_id");

                    b.Property<DateTime>("QuestionEnd")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("question_end");

                    b.Property<long>("QuestionId")
                        .HasColumnType("bigint")
                        .HasColumnName("question_id");

                    b.HasKey("QuestionStart", "StudentTestsId")
                        .HasName("pk_question_times");

                    b.ToTable("question_times", "project");
                });

            modelBuilder.Entity("API.Domain.Entities.Role", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_role");

                    b.ToTable("roles", "project");
                });

            modelBuilder.Entity("API.Domain.Entities.StudentTest", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("GradeId")
                        .HasColumnType("bigint")
                        .HasColumnName("grade_id");

                    b.Property<double>("Points")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("double precision")
                        .HasDefaultValue(0.0)
                        .HasColumnName("points");

                    b.Property<DateTime>("TestFinished")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("test_finished");

                    b.Property<long>("TestId")
                        .HasColumnType("bigint")
                        .HasColumnName("test_id");

                    b.Property<DateTime>("TestStarted")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("test_started");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_student_test");

                    b.HasIndex("GradeId");

                    b.HasIndex("TestId");

                    b.HasIndex("UserId");

                    b.ToTable("student_tests", "project");
                });

            modelBuilder.Entity("API.Domain.Entities.Subject", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_subject");

                    b.ToTable("subjects", "project");
                });

            modelBuilder.Entity("API.Domain.Entities.Test", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<long>("CreatorId")
                        .HasColumnType("bigint")
                        .HasColumnName("creator_id");

                    b.Property<long>("DomainId")
                        .HasColumnType("bigint")
                        .HasColumnName("domain_id");

                    b.Property<long>("MaxPoints")
                        .HasColumnType("bigint")
                        .HasColumnName("max_points");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<bool>("Published")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("published");

                    b.Property<int>("SortBy")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("sort_by");

                    b.Property<long>("SubjectId")
                        .HasColumnType("bigint")
                        .HasColumnName("subject_id");

                    b.Property<long>("TestTimeId")
                        .HasColumnType("bigint")
                        .HasColumnName("test_time_id");

                    b.HasKey("Id")
                        .HasName("pk_test");

                    b.HasIndex("TestTimeId");

                    b.ToTable("tests", "project");
                });

            modelBuilder.Entity("API.Domain.Entities.TestTime", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("End")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("end");

                    b.Property<DateTime>("Start")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("start");

                    b.HasKey("Id")
                        .HasName("pk_test_time");

                    b.ToTable("test_time", "project");
                });

            modelBuilder.Entity("API.Domain.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.Property<string>("Surname")
                        .HasColumnType("text")
                        .HasColumnName("surname");

                    b.Property<string>("Username")
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("pk_user");

                    b.ToTable("users", "project");
                });

            modelBuilder.Entity("API.Domain.Entities.UserRole", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint")
                        .HasColumnName("role_id");

                    b.HasKey("UserId", "RoleId")
                        .HasName("pk_user_role");

                    b.ToTable("user_roles", "project");
                });

            modelBuilder.Entity("API.Domain.Entities.UserSubject", b =>
                {
                    b.Property<long>("SubjectId")
                        .HasColumnType("bigint")
                        .HasColumnName("subject_id");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("SubjectId", "UserId")
                        .HasName("pk_user_subject");

                    b.ToTable("user_subject", "project");
                });

            modelBuilder.Entity("API.Infrastructure.Identity.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.DeviceFlowCodes", b =>
                {
                    b.Property<string>("UserCode")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasMaxLength(50000)
                        .HasColumnType("character varying(50000)");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("DeviceCode")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTime?>("Expiration")
                        .IsRequired()
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("SessionId")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("SubjectId")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("UserCode");

                    b.HasIndex("DeviceCode")
                        .IsUnique();

                    b.HasIndex("Expiration");

                    b.ToTable("DeviceCodes");
                });

            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.PersistedGrant", b =>
                {
                    b.Property<string>("Key")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTime?>("ConsumedTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasMaxLength(50000)
                        .HasColumnType("character varying(50000)");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTime?>("Expiration")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("SessionId")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("SubjectId")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Key");

                    b.HasIndex("Expiration");

                    b.HasIndex("SubjectId", "ClientId", "Type");

                    b.HasIndex("SubjectId", "SessionId", "Type");

                    b.ToTable("PersistedGrants");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("API.Domain.Entities.Answer", b =>
                {
                    b.HasOne("API.Domain.Entities.Question", null)
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("API.Domain.Entities.Question", b =>
                {
                    b.HasOne("API.Domain.Entities.Test", null)
                        .WithMany("Questions")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("API.Domain.Entities.StudentTest", b =>
                {
                    b.HasOne("API.Domain.Entities.Grade", "Grade")
                        .WithMany()
                        .HasForeignKey("GradeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Domain.Entities.Test", "Test")
                        .WithMany()
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Domain.Entities.User", "User")
                        .WithMany("StudentTests")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Grade");

                    b.Navigation("Test");

                    b.Navigation("User");
                });

            modelBuilder.Entity("API.Domain.Entities.Test", b =>
                {
                    b.HasOne("API.Domain.Entities.TestTime", "TestTime")
                        .WithMany()
                        .HasForeignKey("TestTimeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TestTime");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("API.Infrastructure.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("API.Infrastructure.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Infrastructure.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("API.Infrastructure.Identity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("API.Domain.Entities.Question", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("API.Domain.Entities.Test", b =>
                {
                    b.Navigation("Questions");
                });

            modelBuilder.Entity("API.Domain.Entities.User", b =>
                {
                    b.Navigation("StudentTests");
                });
#pragma warning restore 612, 618
        }
    }
}
