DO $$
BEGIN
    -- roles
    IF NOT EXISTS (SELECT 1 FROM roles WHERE role = 'APPLICANT') THEN
        INSERT INTO roles (role) VALUES ('APPLICANT');
    END IF;

    IF NOT EXISTS (SELECT 1 FROM roles WHERE role = 'OFFICER') THEN
        INSERT INTO roles (role) VALUES ('OFFICER');
    END IF;

    IF NOT EXISTS (SELECT 1 FROM roles WHERE role = 'MANAGER') THEN
        INSERT INTO roles (role) VALUES ('MANAGER');
    END IF;

    -- statuses
    IF NOT EXISTS (SELECT 1 FROM application_statuses WHERE name = 'PENDING') THEN
        INSERT INTO application_statuses (name) VALUES ('PENDING');
    END IF;

    IF NOT EXISTS (SELECT 1 FROM application_statuses WHERE name = 'APPROVED') THEN
        INSERT INTO application_statuses (name) VALUES ('APPROVED');
    END IF;

    IF NOT EXISTS (SELECT 1 FROM application_statuses WHERE name = 'REJECTED') THEN
        INSERT INTO application_statuses (name) VALUES ('REJECTED');
    END IF;

    -- travel reasons
    IF NOT EXISTS (SELECT 1 FROM travel_reasons WHERE reason = 'Tourist & Visitor') THEN
        INSERT INTO travel_reasons (reason) VALUES ('Tourist & Visitor');
    END IF;

    IF NOT EXISTS (SELECT 1 FROM travel_reasons WHERE reason = 'Student') THEN
        INSERT INTO travel_reasons (reason) VALUES ('Student');
    END IF;

    IF NOT EXISTS (SELECT 1 FROM travel_reasons WHERE reason = 'Immigration / Settlement') THEN
        INSERT INTO travel_reasons (reason) VALUES ('Immigration / Settlement');
    END IF;

    IF NOT EXISTS (SELECT 1 FROM travel_reasons WHERE reason = 'Business & Investment') THEN
        INSERT INTO travel_reasons (reason) VALUES ('Business & Investment');
    END IF;

    IF NOT EXISTS (SELECT 1 FROM travel_reasons WHERE reason = 'Transit / Crew') THEN
        INSERT INTO travel_reasons (reason) VALUES ('Transit / Crew');
    END IF;

    IF NOT EXISTS (SELECT 1 FROM travel_reasons WHERE reason = 'Special Purpose') THEN
        INSERT INTO travel_reasons (reason) VALUES ('Special Purpose');
    END IF;
END
$$;