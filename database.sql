-- Aden Alexandria Syaiful Perdana
-- 242410102033
-- PAA A

-- CREATE TABLE
-- PASIEN
CREATE TABLE pasien (
	id_pasien SERIAL PRIMARY KEY,
	nama VARCHAR(100) NOT NULL,
	jenis_kelamin VARCHAR(1) CHECK (jenis_kelamin IN ('L','P')),
	tanggal_lahir DATE,
	alamat TEXT,
	no_hp VARCHAR(15),
	created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
	updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
	deleted_at TIMESTAMP NULL
);

CREATE INDEX idx_pasien_nama ON pasien(nama);



-- DOKTER
CREATE TABLE dokter (
	id_dokter SERIAL PRIMARY KEY,
	nama VARCHAR(100) NOT NULL,
	spesialisasi VARCHAR(100),
	no_hp VARCHAR(15),
	created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
	updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);



-- KUNJUNGAN
CREATE TABLE kunjungan (
	id_kunjungan SERIAL PRIMARY KEY,
	id_pasien INT NOT NULL,
	tanggal_kunjungan DATE NOT NULL,
	keluhan TEXT,
	created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
	updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
	CONSTRAINT fk_pasien
		FOREIGN KEY(id_pasien)
		REFERENCES pasien(id_pasien)
		ON DELETE CASCADE
);

CREATE INDEX idx_kunjungan_pasien_id ON kunjungan(id_pasien);



-- REKAM MEDIS
CREATE TABLE rekam_medis (
	id SERIAL PRIMARY KEY,
	id_kunjungan INT NOT NULL,
	id_dokter INT NOT NULL,
	diagnosa TEXT,
	tindakan TEXT,
	biaya DECIMAL(10,2),
	created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
	updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
	CONSTRAINT fk_kunjungan
		FOREIGN KEY(id_kunjungan)
		REFERENCES kunjungan(id_kunjungan)
		ON DELETE CASCADE,
	CONSTRAINT fk_dokter
		FOREIGN KEY(id_dokter)
		REFERENCES dokter(id_dokter)
		ON DELETE CASCADE
);

CREATE INDEX idx_rekam_kunjungan_id ON rekam_medis(id_kunjungan);
CREATE INDEX idx_rekam_dokter_id ON rekam_medis(id_dokter);




-- INSERT TABLE
-- PASIEN
INSERT INTO pasien (nama, jenis_kelamin, tanggal_lahir, alamat, no_hp)
VALUES
('Aden Alexandria', 'L', '2005-05-12', 'Jember', '081234567890'),
('Deswita Selia', 'P', '2005-08-20', 'Ponorogo', '081234567891'),
('Iqbal Rizaldi', 'L', '2006-01-15', 'Sidoarjo', '081234567892'),
('Rossalinda Dwi', 'P', '2007-03-10', 'Banyuwangi', '081234567893'),
('Bintang Royyan', 'L', '2010-11-25', 'Lamongan', '081234567894');


-- DOKTER
INSERT INTO dokter (nama, spesialisasi, no_hp)
VALUES
('Dr. Adrian', 'Umum', '082111111111'),
('Dr. Najwa', 'Gigi', '082111111112'),
('Dr. Tia', 'Anak', '082111111113'),
('Dr. Bagus', 'Penyakit Dalam', '082111111114'),
('Dr. Eka', 'Kulit', '082111111115');


-- KUNJUNGAN
INSERT INTO kunjungan (id_pasien, tanggal_kunjungan, keluhan)
VALUES
(1, '2026-04-01', 'Demam dan batuk'),
(2, '2026-04-02', 'Sakit gigi'),
(3, '2026-04-03', 'Flu'),
(4, '2026-04-04', 'Alergi kulit'),
(5, '2026-04-05', 'Nyeri perut');


-- REKAM MEDIS 
INSERT INTO rekam_medis (id_kunjungan, id_dokter, diagnosa, tindakan, biaya)
VALUES
(1, 1, 'Influenza', 'Obat flu', 50000),
(2, 2, 'Karies gigi', 'Tambal gigi', 150000),
(3, 1, 'Flu ringan', 'Istirahat dan vitamin', 40000),
(4, 5, 'Dermatitis', 'Salep kulit', 75000),
(5, 4, 'Maag', 'Obat lambung', 60000),
(1, 3, 'Flu', 'Obat', 50000);