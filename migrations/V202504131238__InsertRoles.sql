DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM roles WHERE role = 'APPLICANT') THEN
        INSERT INTO roles (role) VALUES ('APPLICANT');
    END IF;

    IF NOT EXISTS (SELECT 1 FROM roles WHERE role = 'OFFICER') THEN
        INSERT INTO roles (role) VALUES ('OFFICER');
    END IF;

    IF NOT EXISTS (SELECT 1 FROM roles WHERE role = 'MANAGER') THEN
        INSERT INTO roles (role) VALUES ('MANAGER');
    END IF;
END
$$;