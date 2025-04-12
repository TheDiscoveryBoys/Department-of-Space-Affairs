ALTER TABLE "passport_application_documents"
ALTER COLUMN "filename" TYPE varchar(200);

ALTER TABLE "passport_application_documents"
ALTER COLUMN "s3_url" TYPE varchar(250);