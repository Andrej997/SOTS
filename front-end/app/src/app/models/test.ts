import { Question } from "./question";

export class Test {
    Id: number;
    Name: string;
    SubjectId: number;
    CreatorId: number;
    MaxPoints: number;
    Questions: Question[];
    
}

export class TestTime {
    Id: number;
    Start: Date;
    End: Date;
}