DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM travel_reasons WHERE reason = 'Tourist & Visitor Visas') THEN
        INSERT INTO travel_reasons (reason) VALUES ('Tourist & Visitor Visas');
    END IF;

    IF NOT EXISTS (SELECT 1 FROM travel_reasons WHERE reason = 'Student Visas') THEN
        INSERT INTO travel_reasons (reason) VALUES ('Student Visas');
    END IF;

    IF NOT EXISTS (SELECT 1 FROM travel_reasons WHERE reason = 'Work Visas') THEN
        INSERT INTO travel_reasons (reason) VALUES ('Work Visas');
    END IF;

    IF NOT EXISTS (SELECT 1 FROM travel_reasons WHERE reason = 'Immigration / Settlement Visas') THEN
        INSERT INTO travel_reasons (reason) VALUES ('Immigration / Settlement Visas');
    END IF;

    IF NOT EXISTS (SELECT 1 FROM travel_reasons WHERE reason = 'Business & Investment Visas') THEN
        INSERT INTO travel_reasons (reason) VALUES ('Business & Investment Visas');
    END IF;

    IF NOT EXISTS (SELECT 1 FROM travel_reasons WHERE reason = 'Transit / Crew Visas') THEN
        INSERT INTO travel_reasons (reason) VALUES ('Transit / Crew Visas');
    END IF;

    IF NOT EXISTS (SELECT 1 FROM travel_reasons WHERE reason = 'Special Purpose / Other Visas') THEN
        INSERT INTO travel_reasons (reason) VALUES ('Special Purpose / Other Visas');
    END IF;
END
$$;