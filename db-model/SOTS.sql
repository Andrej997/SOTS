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

CREATE TABLE "question_times" (
  "question_id" bigint,
  "student_tests_id" bigint,
  "question_start" timestamp,
  "question_end" timestamp,
  PRIMARY KEY ("question_id", "student_tests_id")
);

CREATE TABLE "user_roles" (
  "user_id" bigint,
  "role_id" bigint,
  PRIMARY KEY ("user_id", "role_id")
);

CREATE TABLE "subjects" (
  "id" bigint PRIMARY KEY,
  "name" text,
  "description" text
);

CREATE TABLE "tests" (
  "id" BIGSERIAL PRIMARY KEY,
  "name" varchar,
  "subject_id" bigint,
  "created_at" timestamp,
  "creator_id" bigint,
  "test_time_id" bigint,
  "max_points" int,
  "published" boolean,
  "domain_id" bigint,
  "sort_by" int
);

CREATE TABLE "student_tests" (
  "id" BIGSERIAL PRIMARY KEY,
  "user_id" bigint,
  "test_id" bigint,
  "points" float,
  "grade_id" bigint,
  "test_started" timestamp,
  "test_finished" timestamp
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
  "image" text,
  "created_at" timestamp,
  "points" int,
  "test_id" bigint,
  "problem_node_id" text
);

CREATE TABLE "answers" (
  "id" BIGSERIAL PRIMARY KEY,
  "text_answer" text,
  "question_id" bigint,
  "is_correct" boolean
);

CREATE TABLE "user_subject" (
  "subject_id" bigint,
  "user_id" bigint,
  PRIMARY KEY ("subject_id", "user_id")
);

CREATE TABLE "choosen_answers" (
  "student_test_id" bigint,
  "question_id" bigint,
  "answer_id" bigint,
  "answer_dated" timestamp,
  PRIMARY KEY ("student_test_id", "question_id", "answer_id")
);

CREATE TABLE "nodes" (
  "id" text PRIMARY KEY,
  "label" json,
  "domain_id" bigint
);

CREATE TABLE "edges" (
  "id" text PRIMARY KEY,
  "label" text,
  "source_id" text,
  "target_id" text,
  "domain_id" bigint,
  "date_created" timestamp
);

CREATE TABLE "edges_rk" (
  "id" bigint PRIMARY KEY,
  "test_id" bigint,
  "source_id" text,
  "target_id" text
);

CREATE TABLE "domains" (
  "id" BIGSERIAL PRIMARY KEY,
  "name" text,
  "description" text,
  "subject_id" bigint,
  "date_created" timestamp
);

CREATE TABLE "test_needs_domains" (
  "id" BIGSERIAL PRIMARY KEY,
  "domain_id" bigint,
  "test_id" bigint
);

ALTER TABLE "test_needs_domains" ADD FOREIGN KEY ("domain_id") REFERENCES "domains" ("id");

ALTER TABLE "test_needs_domains" ADD FOREIGN KEY ("test_id") REFERENCES "tests" ("id");

ALTER TABLE "user_roles" ADD FOREIGN KEY ("role_id") REFERENCES "roles" ("id");

ALTER TABLE "user_roles" ADD FOREIGN KEY ("user_id") REFERENCES "users" ("id");

ALTER TABLE "tests" ADD FOREIGN KEY ("subject_id") REFERENCES "subjects" ("id");

ALTER TABLE "student_tests" ADD FOREIGN KEY ("test_id") REFERENCES "tests" ("id");

ALTER TABLE "student_tests" ADD FOREIGN KEY ("user_id") REFERENCES "users" ("id");

ALTER TABLE "tests" ADD FOREIGN KEY ("creator_id") REFERENCES "users" ("id");

ALTER TABLE "tests" ADD FOREIGN KEY ("test_time_id") REFERENCES "test_time" ("id");

ALTER TABLE "student_tests" ADD FOREIGN KEY ("grade_id") REFERENCES "grades" ("id");

ALTER TABLE "questions" ADD FOREIGN KEY ("test_id") REFERENCES "tests" ("id");

ALTER TABLE "answers" ADD FOREIGN KEY ("question_id") REFERENCES "questions" ("id");

ALTER TABLE "choosen_answers" ADD FOREIGN KEY ("student_test_id") REFERENCES "student_tests" ("id");

ALTER TABLE "user_subject" ADD FOREIGN KEY ("subject_id") REFERENCES "subjects" ("id");

ALTER TABLE "user_subject" ADD FOREIGN KEY ("user_id") REFERENCES "users" ("id");

ALTER TABLE "question_times" ADD FOREIGN KEY ("student_tests_id") REFERENCES "student_tests" ("id");

ALTER TABLE "domains" ADD FOREIGN KEY ("id") REFERENCES "subjects" ("id");

ALTER TABLE "domains" ADD FOREIGN KEY ("subject_id") REFERENCES "subjects" ("id");

ALTER TABLE "edges" ADD FOREIGN KEY ("domain_id") REFERENCES "domains" ("id");

ALTER TABLE "nodes" ADD FOREIGN KEY ("domain_id") REFERENCES "domains" ("id");

ALTER TABLE "questions" ADD FOREIGN KEY ("problem_node_id") REFERENCES "nodes" ("id");

ALTER TABLE "edges_rk" ADD FOREIGN KEY ("test_id") REFERENCES "tests" ("id");

