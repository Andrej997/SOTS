export class Question {
    id: number;
    question_id: number;
    TextQuestion: string;
    Points: number;
    Answers: Answer[];

    constructor(id: number) {
        this.question_id = id;
        this.Answers = [];
        this.TextQuestion = '';
        this.Points = 0;
    }
}

export class Answer {
    id: number;
    answer_id: number;
    TextAnswer: string;
    IsCorrect: boolean;

    constructor(id: number) {
        this.answer_id = id;
        this.IsCorrect = false;
        this.TextAnswer = '';
    }
}