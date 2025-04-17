-- Create Users Table (Applicant, Officer, User Manager)
CREATE TABLE "users" (
  "google_id" varchar(30) PRIMARY KEY NOT NULL,
  "email" varchar(255) UNIQUE NOT NULL,
  "name" varchar(255) NOT NULL,
  "species" varchar(50),
  "planet_of_origin" varchar(50),
  "primary_language" varchar(50),
  "date_of_birth" timestamp
);

-- Create Roles Lookup Table
CREATE TABLE "roles" (
  "id" SERIAL PRIMARY KEY,
  "role" varchar(255) UNIQUE NOT NULL -- Added UNIQUE constraint here too for data integrity
);

-- Create User Roles Junction Table (Many-to-Many relationship)
CREATE TABLE "user_roles" (
  "id" SERIAL PRIMARY KEY,
  "user_id" varchar(30) NOT NULL,
  "role_id" int NOT NULL
  -- Removed CONSTRAINT definition from here as it will be added via ALTER TABLE
);

-- Create Statuses Lookup Table
CREATE TABLE "application_statuses" (
  "id" SERIAL PRIMARY KEY,
  "name" varchar(30) UNIQUE NOT NULL -- Added UNIQUE constraint here too for data integrity
);

-- Create Travel Reasons Lookup Table
CREATE TABLE "travel_reasons" (
  "id" SERIAL PRIMARY KEY,
  "reason" varchar(50) UNIQUE NOT NULL -- Added UNIQUE constraint here too for data integrity
);

-- Create Passport Applications Table
CREATE TABLE "passport_applications" (
  "id" SERIAL PRIMARY KEY,
  "user_id" varchar(30) NOT NULL, -- Applicant's ID
  "status_id" int NOT NULL,
  "submitted_at" timestamp NOT NULL DEFAULT (now()),
  "processed_at" timestamp,
  "officer_id" varchar(30), -- Processing Officer's ID
  "officer_comment" varchar(255)
);

-- Create Passport Application Documents Table (One-to-Many with Passport Applications)
CREATE TABLE "passport_application_documents" (
  "id" SERIAL PRIMARY KEY,
  "filename" varchar(255) NOT NULL, -- Increased length slightly
  "s3_url" varchar(255) NOT NULL,  -- Increased length slightly
  "passport_application_id" int NOT NULL
);

-- Create Visa Applications Table
CREATE TABLE "visa_applications" (
  "id" SERIAL PRIMARY KEY,
  "user_id" varchar(30) NOT NULL, -- Applicant's ID
  "destination_planet" varchar(50) NOT NULL,
  "travel_reason_id" int NOT NULL, -- FK to travel_reasons
  "start_date" timestamp NOT NULL,
  "end_date" timestamp NOT NULL,
  "status_id" int NOT NULL,
  "submitted_at" timestamp NOT NULL DEFAULT (now()),
  "processed_at" timestamp,
  "officer_id" varchar(30), -- Processing Officer's ID
  "officer_comment" varchar(255)
);

-- Add Foreign Key Constraints

-- User Roles FKs
ALTER TABLE "user_roles" ADD FOREIGN KEY ("user_id") REFERENCES "users" ("google_id") ON DELETE CASCADE; -- Consider CASCADE or RESTRICT based on your needs
ALTER TABLE "user_roles" ADD FOREIGN KEY ("role_id") REFERENCES "roles" ("id") ON DELETE RESTRICT; -- Prevent deleting roles that are in use

-- Passport Applications FKs
ALTER TABLE "passport_applications" ADD FOREIGN KEY ("user_id") REFERENCES "users" ("google_id") ON DELETE RESTRICT; -- Prevent deleting users with applications (or use SET NULL?)
ALTER TABLE "passport_applications" ADD FOREIGN KEY ("status_id") REFERENCES "application_statuses" ("id") ON DELETE RESTRICT; -- Prevent deleting statuses in use
ALTER TABLE "passport_applications" ADD FOREIGN KEY ("officer_id") REFERENCES "users" ("google_id") ON DELETE SET NULL; -- If officer user is deleted, set officer_id to NULL

-- Passport Application Documents FK
ALTER TABLE "passport_application_documents" ADD FOREIGN KEY ("passport_application_id") REFERENCES "passport_applications" ("id") ON DELETE CASCADE; -- If application is deleted, delete its documents

-- Visa Applications FKs
ALTER TABLE "visa_applications" ADD FOREIGN KEY ("user_id") REFERENCES "users" ("google_id") ON DELETE RESTRICT; -- Prevent deleting users with applications (or use SET NULL?)
ALTER TABLE "visa_applications" ADD FOREIGN KEY ("status_id") REFERENCES "application_statuses" ("id") ON DELETE RESTRICT; -- Prevent deleting statuses in use
ALTER TABLE "visa_applications" ADD FOREIGN KEY ("officer_id") REFERENCES "users" ("google_id") ON DELETE SET NULL; -- If officer user is deleted, set officer_id to NULL
ALTER TABLE "visa_applications" ADD FOREIGN KEY ("travel_reason_id") REFERENCES "travel_reasons" ("id") ON DELETE RESTRICT; -- Prevent deleting reasons in use