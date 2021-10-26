export class Question {
    id: number;
    TextQuestion: string;
    Answers: Answer[];

    constructor(id: number) {
        this.id = id;
        this.Answers = [];
    }
}

export class Answer {
    id: number;
    TextAnswer: string;
    IsCorrect: boolean;

    constructor(id: number) {
        this.id = id;
        this.IsCorrect = false;
    }
}