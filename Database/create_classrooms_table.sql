-- ====================================
-- H? TH?NG QU?N LÝ TR??NG H?C
-- T?o các b?ng d? li?u trên Supabase
-- ====================================

-- 1. B?ng l?p h?c (Classrooms)
CREATE TABLE IF NOT EXISTS public.classrooms (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    description VARCHAR(500),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

-- 2. B?ng h?c sinh (Students)
CREATE TABLE IF NOT EXISTS public.students (
    id SERIAL PRIMARY KEY,
    student_code VARCHAR(20) NOT NULL UNIQUE,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    date_of_birth DATE,
    gender VARCHAR(10),
    email VARCHAR(100),
    phone VARCHAR(20),
    address VARCHAR(200),
    parent_name VARCHAR(100),
    parent_phone VARCHAR(20),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

-- 3. B?ng môn h?c (Subjects)
CREATE TABLE IF NOT EXISTS public.subjects (
    id SERIAL PRIMARY KEY,
    subject_code VARCHAR(20) NOT NULL UNIQUE,
    name VARCHAR(100) NOT NULL,
    description VARCHAR(500),
    credits INTEGER DEFAULT 1,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

-- 4. B?ng quan h? h?c sinh - l?p h?c (Classroom Students)
CREATE TABLE IF NOT EXISTS public.classroom_students (
    id SERIAL PRIMARY KEY,
    classroom_id INTEGER NOT NULL REFERENCES public.classrooms(id) ON DELETE CASCADE,
    student_id INTEGER NOT NULL REFERENCES public.students(id) ON DELETE CASCADE,
    seat_number VARCHAR(10),
    enrollment_date TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    status VARCHAR(20) DEFAULT 'active', -- active, inactive, transferred
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(classroom_id, student_id)
);

-- 5. B?ng ?i?m s? (Scores)
CREATE TABLE IF NOT EXISTS public.scores (
    id SERIAL PRIMARY KEY,
    student_id INTEGER NOT NULL REFERENCES public.students(id) ON DELETE CASCADE,
    subject_id INTEGER NOT NULL REFERENCES public.subjects(id) ON DELETE CASCADE,
    classroom_id INTEGER NOT NULL REFERENCES public.classrooms(id) ON DELETE CASCADE,
    score_type VARCHAR(20) NOT NULL, -- oral, 15min, 45min, midterm, final
    score_value DECIMAL(4,2) NOT NULL CHECK (score_value >= 0 AND score_value <= 10),
    coefficient INTEGER DEFAULT 1 CHECK (coefficient >= 1 AND coefficient <= 3),
    semester INTEGER NOT NULL CHECK (semester IN (1, 2)),
    academic_year VARCHAR(10) NOT NULL, -- e.g., "2024-2025"
    notes VARCHAR(500),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

-- ====================================
-- T?O INDEX ?? T?I ?U HÓA TRUY V?N
-- ====================================

-- Classrooms indexes
CREATE INDEX IF NOT EXISTS idx_classrooms_created_at ON public.classrooms(created_at DESC);
CREATE INDEX IF NOT EXISTS idx_classrooms_name ON public.classrooms(name);

-- Students indexes
CREATE INDEX IF NOT EXISTS idx_students_created_at ON public.students(created_at DESC);
CREATE INDEX IF NOT EXISTS idx_students_student_code ON public.students(student_code);
CREATE INDEX IF NOT EXISTS idx_students_name ON public.students(last_name, first_name);

-- Subjects indexes
CREATE INDEX IF NOT EXISTS idx_subjects_created_at ON public.subjects(created_at DESC);
CREATE INDEX IF NOT EXISTS idx_subjects_subject_code ON public.subjects(subject_code);

-- Classroom Students indexes
CREATE INDEX IF NOT EXISTS idx_classroom_students_classroom ON public.classroom_students(classroom_id);
CREATE INDEX IF NOT EXISTS idx_classroom_students_student ON public.classroom_students(student_id);
CREATE INDEX IF NOT EXISTS idx_classroom_students_status ON public.classroom_students(status);

-- Scores indexes
CREATE INDEX IF NOT EXISTS idx_scores_student ON public.scores(student_id);
CREATE INDEX IF NOT EXISTS idx_scores_subject ON public.scores(subject_id);
CREATE INDEX IF NOT EXISTS idx_scores_classroom ON public.scores(classroom_id);
CREATE INDEX IF NOT EXISTS idx_scores_academic_year ON public.scores(academic_year);

-- ====================================
-- THÊM COMMENT CHO CÁC B?NG VÀ C?T
-- ====================================

-- Classrooms
COMMENT ON TABLE public.classrooms IS 'B?ng l?u tr? thông tin các l?p h?c';
COMMENT ON COLUMN public.classrooms.id IS 'ID duy nh?t c?a l?p h?c';
COMMENT ON COLUMN public.classrooms.name IS 'Tên l?p h?c';
COMMENT ON COLUMN public.classrooms.description IS 'Mô t? l?p h?c';

-- Students
COMMENT ON TABLE public.students IS 'B?ng l?u tr? thông tin h?c sinh';
COMMENT ON COLUMN public.students.student_code IS 'Mã h?c sinh (unique)';
COMMENT ON COLUMN public.students.first_name IS 'Tên h?c sinh';
COMMENT ON COLUMN public.students.last_name IS 'H? và tên ??m h?c sinh';
COMMENT ON COLUMN public.students.parent_name IS 'Tên ph? huynh';
COMMENT ON COLUMN public.students.parent_phone IS 'S? ?i?n tho?i ph? huynh';

-- Subjects
COMMENT ON TABLE public.subjects IS 'B?ng l?u tr? thông tin môn h?c';
COMMENT ON COLUMN public.subjects.subject_code IS 'Mã môn h?c (unique)';
COMMENT ON COLUMN public.subjects.credits IS 'S? tín ch? c?a môn h?c';

-- Classroom Students
COMMENT ON TABLE public.classroom_students IS 'B?ng quan h? gi?a h?c sinh và l?p h?c';
COMMENT ON COLUMN public.classroom_students.seat_number IS 'S? ch? ng?i c?a h?c sinh trong l?p';
COMMENT ON COLUMN public.classroom_students.status IS 'Tr?ng thái: active, inactive, transferred';

-- Scores
COMMENT ON TABLE public.scores IS 'B?ng l?u tr? ?i?m s? c?a h?c sinh';
COMMENT ON COLUMN public.scores.score_type IS 'Lo?i ?i?m: oral, 15min, 45min, midterm, final';
COMMENT ON COLUMN public.scores.coefficient IS 'H? s? ?i?m (1-3)';
COMMENT ON COLUMN public.scores.semester IS 'H?c k? (1 ho?c 2)';
COMMENT ON COLUMN public.scores.academic_year IS 'N?m h?c (vd: 2024-2025)';

-- ====================================
-- THÊM D? LI?U M?U (Optional - có th? xóa n?u không c?n)
-- ====================================

-- L?p h?c m?u
INSERT INTO public.classrooms (name, description) 
VALUES
    ('10A1', 'Lop 10 ban Toan - Tin'),
    ('11B2', 'Lop 11 ban Xa hoi'),
    ('12C3', 'Lop 12 ban Tu nhien')
ON CONFLICT DO NOTHING;

-- Môn h?c m?u
INSERT INTO public.subjects (subject_code, name, description, credits) 
VALUES
    ('TOAN', 'Toan hoc', 'Mon Toan hoc pho thong', 3),
    ('VAN', 'Ngu van', 'Mon Ngu van Viet Nam', 3),
    ('ANH', 'Tieng Anh', 'Mon Tieng Anh', 2),
    ('LY', 'Vat ly', 'Mon Vat ly', 2),
    ('HOA', 'Hoa hoc', 'Mon Hoa hoc', 2),
    ('SINH', 'Sinh hoc', 'Mon Sinh hoc', 2),
    ('SU', 'Lich su', 'Mon Lich su', 2),
    ('DIA', 'Dia ly', 'Mon Dia ly', 2)
ON CONFLICT DO NOTHING;

-- H?c sinh m?u
INSERT INTO public.students (student_code, first_name, last_name, date_of_birth, gender, email, phone, address, parent_name, parent_phone) 
VALUES
    ('HS001', 'Van An', 'Nguyen', '2008-05-15', 'Nam', 'nguyenvanan@example.com', '0901234567', 'Ha Noi', 'Nguyen Van Binh', '0912345678'),
    ('HS002', 'Thi Binh', 'Tran', '2008-08-20', 'Nu', 'tranthibinh@example.com', '0902345678', 'Hai Phong', 'Tran Van Cuong', '0923456789'),
    ('HS003', 'Van Cuong', 'Le', '2008-03-10', 'Nam', 'levancuong@example.com', '0903456789', 'Da Nang', 'Le Thi Dung', '0934567890')
ON CONFLICT DO NOTHING;

-- Gán h?c sinh vào l?p
INSERT INTO public.classroom_students (classroom_id, student_id, seat_number, status) 
VALUES
    (1, 1, 'A01', 'active'),
    (1, 2, 'A02', 'active'),
    (2, 3, 'B01', 'active')
ON CONFLICT DO NOTHING;

-- ?i?m s? m?u
INSERT INTO public.scores (student_id, subject_id, classroom_id, score_type, score_value, coefficient, semester, academic_year) 
VALUES
    (1, 1, 1, 'oral', 8.5, 1, 1, '2024-2025'),
    (1, 1, 1, '15min', 9.0, 1, 1, '2024-2025'),
    (1, 1, 1, 'midterm', 8.0, 2, 1, '2024-2025'),
    (2, 2, 1, 'oral', 7.5, 1, 1, '2024-2025'),
    (2, 2, 1, '15min', 8.0, 1, 1, '2024-2025')
ON CONFLICT DO NOTHING;

-- ====================================
-- HOÀN T?T
-- ====================================
-- Ch?y script này trong SQL Editor c?a Supabase
-- Sau khi ch?y xong, ki?m tra l?i các b?ng trong Table Editor
