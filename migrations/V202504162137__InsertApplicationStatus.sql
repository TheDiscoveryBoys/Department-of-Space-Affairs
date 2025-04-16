DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM application_statuses WHERE name = 'PENDING') THEN
        INSERT INTO application_statuses (name) VALUES ('PENDING');
    END IF;

    IF NOT EXISTS (SELECT 1 FROM application_statuses WHERE name = 'ACCEPTED') THEN
        INSERT INTO application_statuses (name) VALUES ('ACCEPTED');
    END IF;

    IF NOT EXISTS (SELECT 1 FROM application_statuses WHERE name = 'REJECTED') THEN
        INSERT INTO application_statuses (name) VALUES ('REJECTED');
    END IF;
END
$$;