DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM travel_reasons WHERE reason = 'Tourist & Visitor') THEN
        INSERT INTO travel_reasons (reason) VALUES ('Tourist & Visitor');
    END IF;

    IF NOT EXISTS (SELECT 1 FROM travel_reasons WHERE reason = 'Student') THEN
        INSERT INTO travel_reasons (reason) VALUES ('Student');
    END IF;

    IF NOT EXISTS (SELECT 1 FROM travel_reasons WHERE reason = 'Work') THEN
        INSERT INTO travel_reasons (reason) VALUES ('Work');
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

    IF NOT EXISTS (SELECT 1 FROM travel_reasons WHERE reason = 'Special Purpose / Other') THEN
        INSERT INTO travel_reasons (reason) VALUES ('Special Purpose / Other');
    END IF;
END
$$;