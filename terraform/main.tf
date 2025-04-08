terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }

  backend "s3" {        
    region         = "af-south-1"
  }
}

provider "aws" {
  region =  "af-south-1"
}

resource "aws_default_vpc" "default_vpc" {
  tags = {
    Name = "default_vpc"
  }
}

data "aws_availability_zones" "available_zones" {
  
}

resource "aws_default_subnet" "subnet_az1" {
  availability_zone = data.aws_availability_zones.available_zones.names[0]
}

resource "aws_default_subnet" "subnet_az2" {
  availability_zone = data.aws_availability_zones.available_zones.names[1]
}

resource "aws_security_group" "allow_postgres" {
  name_prefix = "allow_postgres_"

  ingress {
    from_port   = 5432
    to_port     = 5432
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

data "aws_secretsmanager_secret_version" "postgresuser" {
  secret_id = "postgresuser"
}

data "aws_secretsmanager_secret_version" "postgrespass" {
  secret_id = "postgrespass"
}

resource "aws_db_instance" "spaceaffairsdb" {
  identifier             = "spaceaffairsdb"
  engine                 = "postgres"
  engine_version         = "16.4"
  instance_class         = "db.t4g.micro"
  db_name                = "spaceaffairsdb"
  allocated_storage      = 20
  storage_type           = "gp2"
  publicly_accessible    = true
  username               = data.aws_secretsmanager_secret_version.postgresuser.secret_string
  password               = data.aws_secretsmanager_secret_version.postgrespass.secret_string
  skip_final_snapshot    = true
  vpc_security_group_ids = [aws_security_group.allow_postgres.id]
  tags = {
    Name = "spaceaffairsdb"
  }
}

output "db_host" {
  value = aws_db_instance.spaceaffairsdb.endpoint
  description = "The endpoint of the Postgres Server RDS instance"
}

resource "aws_s3_bucket" "testspaceaffairsbucket123" {
  bucket = "testspaceaffairsbucket123"

}

resource "aws_security_group" "ec2_security_group" {
  name_prefix = "spaceaffairs_api_sg"

  ingress {
    from_port   = 22
    to_port     = 22
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
  ingress {
    from_port   = 443
    to_port     = 443
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

resource "aws_instance" "spaceaffairs_ec2_instance" {
  ami           = "ami-036e83870e09b7396"
  instance_type = "t3.micro"
  key_name      = "spaceaffairs-key"
  tags = {
    Name = "spaceaffairs_ec2_instance"
  }

  vpc_security_group_ids = [ aws_security_group.ec2_security_group.id ]

  user_data = <<-EOF
        <powershell>
        $pubkey = "${replace(file("spaceaffairspublic.pub"), "\n", "")}"
        mkdir "C:\\ProgramData\\ssh"
        echo $pubkey | Out-File -FilePath "C:\\ProgramData\\ssh\\administrators_authorized_keys" -Encoding ascii
        icacls "C:\\ProgramData\\ssh\\administrators_authorized_keys" /inheritance:r
        icacls "C:\\ProgramData\\ssh\\administrators_authorized_keys" /grant "Administrators:F"
        Restart-Service sshd
        </powershell>
        EOF
}

resource "aws_eip" "spaceaffairs_ec2_eip" {
  instance = aws_instance.spaceaffairs_ec2_instance.id
  domain   = "vpc"
}

output "ec2_host" {
  value = aws_eip.spaceaffairs_ec2_eip.public_dns
  description = "The endpoint of the EC2 instance"
}