# Use S3 bucket to store the state file
resource "aws_s3_bucket" "terraform_state" {
  bucket = "SpaceAffairsS3Bucket"
}

# Use dynamoDB to enable state locking
resource "aws_dynamodb_table" "terraform_state_lock" {
  name           = "state-lock"
  read_capacity  = 1
  write_capacity = 1
  hash_key       = "LockID"

  attribute {
    name = "LockID"
    type = "S"
  }
}