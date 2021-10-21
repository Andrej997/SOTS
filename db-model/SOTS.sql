CREATE TABLE "users" (
  "id" BIGSERIAL PRIMARY KEY,
  "name" text,
  "surname" text,
  "username" text,
  "password_hash" text,
  "created_at" timestamp
);

CREATE TABLE "roles" (
  "id" bigint PRIMARY KEY,
  "name" varchar
);

CREATE TABLE "user_roles" (
  "user_id" bigint,
  "role_id" bigint,
  PRIMARY KEY ("user_id", "role_id")
);

CREATE TABLE "subjects" (
  "id" bigint PRIMARY KEY,
  "name" text
);

CREATE TABLE "tests" (
  "id" BIGSERIAL PRIMARY KEY,
  "name" varchar,
  "subject_id" bigint,
  "created_at" timestamp,
  "creator_id" bigint,
  "test_time_id" bigint
);

CREATE TABLE "student_tests" (
  "user_id" bigint,
  "test_id" bigint,
  "took_test" boolean,
  "grade_id" bigint,
  PRIMARY KEY ("user_id", "test_id")
);

CREATE TABLE "test_time" (
  "id" bigint PRIMARY KEY,
  "start" timestamp,
  "end" timestamp
);

CREATE TABLE "grades" (
  "id" [pk],
  "label" text
);

CREATE TABLE "questions" (
  "id" BIGSERIAL PRIMARY KEY,
  "question" text,
  "created_at" timestamp
);

CREATE TABLE "answers" (
  "id" BIGSERIAL PRIMARY KEY,
  "answer" text
);

CREATE TABLE "questions_answers" (
  "question_id" bigint,
  "answer_id" bigint,
  PRIMARY KEY ("question_id", "answer_id")
);

CREATE TABLE "test_questions" (
  "test_id" bigint,
  "question_id" bigint,
  PRIMARY KEY ("test_id", "question_id")
);

ALTER TABLE "user_roles" ADD FOREIGN KEY ("role_id") REFERENCES "roles" ("id");

ALTER TABLE "user_roles" ADD FOREIGN KEY ("user_id") REFERENCES "users" ("id");

ALTER TABLE "tests" ADD FOREIGN KEY ("subject_id") REFERENCES "subjects" ("id");

ALTER TABLE "student_tests" ADD FOREIGN KEY ("test_id") REFERENCES "tests" ("id");

ALTER TABLE "student_tests" ADD FOREIGN KEY ("user_id") REFERENCES "users" ("id");

ALTER TABLE "tests" ADD FOREIGN KEY ("creator_id") REFERENCES "users" ("id");

ALTER TABLE "tests" ADD FOREIGN KEY ("test_time_id") REFERENCES "test_time" ("id");

ALTER TABLE "student_tests" ADD FOREIGN KEY ("grade_id") REFERENCES "grades" ("id");

ALTER TABLE "questions_answers" ADD FOREIGN KEY ("question_id") REFERENCES "questions" ("id");

ALTER TABLE "questions_answers" ADD FOREIGN KEY ("answer_id") REFERENCES "answers" ("id");

ALTER TABLE "test_questions" ADD FOREIGN KEY ("question_id") REFERENCES "questions" ("id");

ALTER TABLE "test_questions" ADD FOREIGN KEY ("test_id") REFERENCES "tests" ("id");

