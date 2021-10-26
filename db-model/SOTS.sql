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
  "id" BIGSERIAL PRIMARY KEY,
  "user_id" bigint,
  "test_id" bigint,
  "took_test" boolean,
  "points" float,
  "grade_id" bigint
);

CREATE TABLE "tests_test_time" (
  "test_id" bigint,
  "test_time_id" bigint,
  PRIMARY KEY ("test_id", "test_time_id")
);

CREATE TABLE "test_time" (
  "id" bigint PRIMARY KEY,
  "start" timestamp,
  "end" timestamp
);

CREATE TABLE "grades" (
  "id" bigint PRIMARY KEY,
  "from_procentage" float,
  "to_procentage" float,
  "label" text
);

CREATE TABLE "questions" (
  "id" BIGSERIAL PRIMARY KEY,
  "text_question" text,
  "created_at" timestamp,
  "test_id" bigint
);

CREATE TABLE "answers" (
  "id" BIGSERIAL PRIMARY KEY,
  "text_answer" text,
  "question_id" bigint,
  "is_correct" boolean
);

CREATE TABLE "question_completed" (
  "id" bigint PRIMARY KEY,
  "student_tests_id" bigint,
  "question_id" bigint,
  "completed_persentage" fload
);

CREATE TABLE "user_subject" (
  "subject_id" bigint,
  "user_id" bigint,
  PRIMARY KEY ("subject_id", "user_id")
);

ALTER TABLE "user_roles" ADD FOREIGN KEY ("role_id") REFERENCES "roles" ("id");

ALTER TABLE "user_roles" ADD FOREIGN KEY ("user_id") REFERENCES "users" ("id");

ALTER TABLE "tests" ADD FOREIGN KEY ("subject_id") REFERENCES "subjects" ("id");

ALTER TABLE "student_tests" ADD FOREIGN KEY ("test_id") REFERENCES "tests" ("id");

ALTER TABLE "student_tests" ADD FOREIGN KEY ("user_id") REFERENCES "users" ("id");

ALTER TABLE "tests" ADD FOREIGN KEY ("creator_id") REFERENCES "users" ("id");

ALTER TABLE "tests_test_time" ADD FOREIGN KEY ("test_id") REFERENCES "tests" ("test_time_id");

ALTER TABLE "tests_test_time" ADD FOREIGN KEY ("test_time_id") REFERENCES "test_time" ("id");

ALTER TABLE "student_tests" ADD FOREIGN KEY ("grade_id") REFERENCES "grades" ("id");

ALTER TABLE "questions" ADD FOREIGN KEY ("test_id") REFERENCES "tests" ("id");

ALTER TABLE "answers" ADD FOREIGN KEY ("question_id") REFERENCES "questions" ("id");

ALTER TABLE "question_completed" ADD FOREIGN KEY ("student_tests_id") REFERENCES "student_tests" ("id");

ALTER TABLE "question_completed" ADD FOREIGN KEY ("question_id") REFERENCES "questions" ("id");

ALTER TABLE "user_subject" ADD FOREIGN KEY ("subject_id") REFERENCES "subjects" ("id");

ALTER TABLE "user_subject" ADD FOREIGN KEY ("user_id") REFERENCES "users" ("id");

