CREATE TABLE "users" (
  "google_id" varchar(30) PRIMARY KEY NOT NULL,
  "email" varchar(255) UNIQUE NOT NULL,
  "name" varchar(255) NOT NULL,
  "species" varchar(50),
  "planet_of_origin" varchar(50),
  "primary_language" varchar(50),
  "date_of_birth" timestamp
);
 
CREATE TABLE "user_roles" (
  "id" serial PRIMARY KEY,
  "user_id" varchar(30) NOT NULL,
  "role_id" int NOT NULL
);
 
CREATE TABLE "roles" (
  "id" serial PRIMARY KEY,
  "role" varchar(255) NOT NULL
);
 
CREATE TABLE "statuses" (
  "id" serial PRIMARY KEY,
  "name" varchar(30) NOT NULL,
  "reason" varchar(255)
);
 
CREATE TABLE "passport_applications" (
  "id" serial PRIMARY KEY,
  "user_id" varchar(30) NOT NULL,
  "status_id" int NOT NULL,
  "submitted_at" timestamp NOT NULL DEFAULT (now()),
  "processed_at" timestamp,
  "officer_id" varchar(30)
);
 
CREATE TABLE "passport_application_documents" (
  "id" serial PRIMARY KEY,
  "filename" varchar(50) NOT NULL,
  "s3_url" varchar(100) NOT NULL,
  "passport_application_id" int NOT NULL
);
 
CREATE TABLE "visa_applications" (
  "id" serial PRIMARY KEY,
  "user_id" varchar(30) NOT NULL,
  "destination_planet" varchar(50) NOT NULL,
  "travel_reason" varchar(255) NOT NULL,
  "start_date" timestamp NOT NULL,
  "end_date" timestamp NOT NULL,
  "status_id" int NOT NULL,
  "submitted_at" timestamp NOT NULL DEFAULT (now()),
  "processed_at" timestamp,
  "officer_id" varchar(30)
);
 
ALTER TABLE "user_roles" ADD FOREIGN KEY ("user_id") REFERENCES "users" ("google_id");
 
ALTER TABLE "user_roles" ADD FOREIGN KEY ("role_id") REFERENCES "roles" ("id");
 
ALTER TABLE "passport_applications" ADD FOREIGN KEY ("user_id") REFERENCES "users" ("google_id");
 
ALTER TABLE "passport_applications" ADD FOREIGN KEY ("status_id") REFERENCES "statuses" ("id");
 
ALTER TABLE "passport_applications" ADD FOREIGN KEY ("officer_id") REFERENCES "users" ("google_id");
 
ALTER TABLE "passport_application_documents" ADD FOREIGN KEY ("passport_application_id") REFERENCES "passport_applications" ("id");
 
ALTER TABLE "visa_applications" ADD FOREIGN KEY ("user_id") REFERENCES "users" ("google_id");
 
ALTER TABLE "visa_applications" ADD FOREIGN KEY ("status_id") REFERENCES "statuses" ("id");
 
ALTER TABLE "visa_applications" ADD FOREIGN KEY ("officer_id") REFERENCES "users" ("google_id");