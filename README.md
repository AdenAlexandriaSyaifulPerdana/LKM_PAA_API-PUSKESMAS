# REST API Puskesmas

## Author

* Nama  : Aden Alexandria Syaiful Perdana
* NIM   : 242410102033
* Kelas : PAA A

---

## Deskripsi Project

Project ini merupakan implementasi REST API Sistem Informasi Puskesmas yang digunakan untuk mengelola data:

1. Pasien
2. Dokter
3. Kunjungan
4. Rekam Medis

API ini mendukung operasi CRUD (Create, Read, Update, Delete) dan dirancang untuk membantu proses digitalisasi layanan kesehatan di puskesmas agar lebih efisien, terstruktur, dan mudah diakses.

---

## Teknologi yang Digunakan

1. Bahasa Pemrograman: C#
2. Framework: ASP.NET Core Web API
3. Database: PostgreSQL
4. Library: Npgsql (PostgreSQL Driver)
5. Tools: Visual Studio, Swagger (OpenAPI)

---

## Cara Instalasi dan Menjalankan Project

### 1. Clone Repository

```bash
git clone https://github.com/AdenAlexandriaSyaifulPerdana/LKM_PAA_API-PUSKESMAS
cd LKM_PAA_API-PUSKESMAS
```

### 2. Buka Project

Buka file solution (.sln) menggunakan Visual Studio.

### 3. Konfigurasi Database

Edit file `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=nama_db;Username=postgres;Password=123"
}
```

### 4. Jalankan Project

Tekan F5 atau klik Start pada Visual Studio.
API akan berjalan dan dapat diakses melalui:

```
https://localhost:xxxx/swagger
```

---

## Cara Import Database

1. Buka pgAdmin atau PostgreSQL
2. Buat database baru
3. Buka Query Tool
4. Jalankan file `database.sql`

File tersebut sudah berisi:

1. Struktur tabel (DDL)
2. Relasi foreign key
3. Sample data minimal 5 baris per tabel

---

## Daftar Endpoint API

### Pasien

| Method | Endpoint         | Keterangan                 |
| ------ | ---------------- | -------------------------- |
| GET    | /api/pasien      | Ambil semua data pasien    |
| GET    | /api/pasien/{id} | Ambil detail pasien        |
| POST   | /api/pasien      | Tambah pasien              |
| PUT    | /api/pasien/{id} | Update pasien              |
| DELETE | /api/pasien/{id} | Hapus pasien (soft delete) |

### Dokter

| Method | Endpoint         | Keterangan         |
| ------ | ---------------- | ------------------ |
| GET    | /api/dokter      | Ambil semua dokter |
| GET    | /api/dokter/{id} | Detail dokter      |
| POST   | /api/dokter      | Tambah dokter      |
| PUT    | /api/dokter/{id} | Update dokter      |
| DELETE | /api/dokter/{id} | Hapus dokter       |

### Kunjungan

| Method | Endpoint                   | Keterangan                   |
| ------ | -------------------------- | ---------------------------- |
| GET    | /api/kunjungan             | Ambil semua kunjungan        |
| GET    | /api/kunjungan/pasien/{id} | Kunjungan berdasarkan pasien |
| POST   | /api/kunjungan             | Tambah kunjungan             |

### Rekam Medis

| Method | Endpoint                    | Keterangan                     |
| ------ | --------------------------- | ------------------------------ |
| GET    | /api/rekammedis             | Ambil semua rekam medis        |
| GET    | /api/rekammedis/dokter/{id} | Rekam medis berdasarkan dokter |
| POST   | /api/rekammedis             | Tambah rekam medis             |

---

## Format Response API

### Success

```json
{
  "status": "success",
  "data": {},
  "meta": {}
}
```

### Error

```json
{
  "status": "error",
  "message": "Pesan error"
}
```

---

## Link Video Presentasi

https://youtu.be/fRTnQCsnz_8
