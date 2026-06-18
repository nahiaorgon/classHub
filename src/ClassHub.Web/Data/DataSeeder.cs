using ClassHub.Web.Models;

namespace ClassHub.Web.Data;

public static class DataSeeder
{
    public static void Seed(AppDbContext db, string contentRootPath)
    {
        if (db.Users.Any()) return;

        var profesor = new AppUser
        {
            Name = "Profesor Demo",
            Email = "profesor@classhub.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("profesor123"),
            Role = UserRole.Profesor
        };
        var alumno = new AppUser
        {
            Name = "Alumno Demo",
            Email = "alumno@classhub.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("alumno123"),
            Role = UserRole.Alumno
        };
        db.Users.AddRange(profesor, alumno);

        var u1 = new Unit { Number = 1, Title = "Cambiado a literal diferente - Jueves 18/06",                    Description = "Introducción a la IS, historia, crisis del software, modelos de proceso y ciclos de vida." };
        var u2 = new Unit { Number = 2, Title = "Gestión del Software como producto",                    Description = "Gestión de proyectos, estimación, planificación, métricas y control de calidad." };
        var u3 = new Unit { Number = 3, Title = "Gestión Ágil de Proyectos",                             Description = "Marcos ágiles (Scrum, Kanban), sprints, historias de usuario e integración continua." };
        var u4 = new Unit { Number = 4, Title = "Aseguramiento de Calidad del Producto del Software",    Description = "Testing, revisiones, métricas de calidad, SonarQube y estándares ISO." };
        db.Units.AddRange(u1, u2, u3, u4);
        db.SaveChanges();

        // ── Copiar PDF de guía bibliográfica a la carpeta de uploads ──────────────
        var uploadsDir = Path.Combine(contentRootPath, "uploads", "recursos", u1.Id.ToString());
        Directory.CreateDirectory(uploadsDir);
        var seedPdf = Path.Combine(contentRootPath, "SeedData", "guia-bibliografia-u1.pdf");
        var destPdf = Path.Combine(uploadsDir, "guia-bibliografia-u1.pdf");
        if (File.Exists(seedPdf) && !File.Exists(destPdf))
            File.Copy(seedPdf, destPdf);

        // ── Recursos Unidad 1 · Ingeniería de Software en Contexto ────────────────
        // Clasificación según guía bibliográfica 2026:
        //   🔴 OBLIGATORIO  = texto en rojo en la guía
        //   🔵 COMPLEMENTARIO = texto en azul (links/PDFs descargables) en la guía
        var recursos = new List<Resource>
        {
            // PDF de la guía completa
            new() { UnitId = u1.Id,
                    Title = "Guía Bibliográfica Unidad 1 — 2026",
                    Description = "Documento oficial con todo el material obligatorio y complementario de la unidad. Subido por la cátedra.",
                    Type = ResourceType.Pdf, FileName = "guia-bibliografia-u1.pdf",
                    FilePath = destPdf,
                    UploadedById = profesor.Id, CreatedAt = DateTime.UtcNow },

            // ── Tema 1: Introducción a la Ingeniería del Software ─────────────────
            new() { UnitId = u1.Id,
                    Title = "🔴 Fowler & Beck — Frameworks for reinventing software, again and again",
                    Description = "OBLIGATORIO · Tema 1. Artículo de Fowler y Beck sobre frameworks y reinvención continua del software.",
                    Type = ResourceType.Link, Url = "https://martinfowler.com",
                    UploadedById = profesor.Id, CreatedAt = DateTime.UtcNow },

            new() { UnitId = u1.Id,
                    Title = "🔴 IEEE SWEBOK v3 — Guide to the Software Engineering Body of Knowledge",
                    Description = "OBLIGATORIO · Tema 1. Cuerpo de conocimiento oficial de la IS publicado por IEEE (2004). Los Alamitos, California, USA.",
                    Type = ResourceType.Link, Url = "https://www.computer.org/education/bodies-of-knowledge/software-engineering",
                    UploadedById = profesor.Id, CreatedAt = DateTime.UtcNow },

            new() { UnitId = u1.Id,
                    Title = "🔵 SWEBOKv3.pdf — Descarga directa",
                    Description = "COMPLEMENTARIO · Tema 1. Archivo PDF del SWEBOK v3 disponible en ieeesociety (también hay resumen en PPT).",
                    Type = ResourceType.Link, Url = "https://www.computer.org/education/bodies-of-knowledge/software-engineering/v3",
                    UploadedById = profesor.Id, CreatedAt = DateTime.UtcNow },

            new() { UnitId = u1.Id,
                    Title = "🔴 Sommerville — Ingeniería de Software 9ª ed. · Cap. 1, 22, 23",
                    Description = "OBLIGATORIO · Tema 1. Cap. 1: introducción; Cap. 22: gestión de la calidad; Cap. 23: mejora de procesos. Ed. Addison-Wesley 2011.",
                    Type = ResourceType.Link, Url = "https://www.pearson.com/en-us/subject-catalog/p/software-engineering/P200000003268",
                    UploadedById = profesor.Id, CreatedAt = DateTime.UtcNow },

            new() { UnitId = u1.Id,
                    Title = "🔴 Pressman — Ingeniería de Software: Un Enfoque Práctico 7ª ed. · Cap. 1",
                    Description = "OBLIGATORIO · Tema 1. Cap. 1: La naturaleza del software y la ingeniería del software. Ed. McGraw-Hill 2010.",
                    Type = ResourceType.Link, Url = "https://www.mheducation.com",
                    UploadedById = profesor.Id, CreatedAt = DateTime.UtcNow },

            new() { UnitId = u1.Id,
                    Title = "🔵 Pressman — Enlace de descarga del libro",
                    Description = "COMPLEMENTARIO · Tema 1. Acceso al libro de Pressman — Ingeniería de Software, Un Enfoque Práctico.",
                    Type = ResourceType.Link, Url = "https://www.mheducation.com/highered/product/software-engineering-practitioner-s-approach-pressman-maxim/M9781259872976.html",
                    UploadedById = profesor.Id, CreatedAt = DateTime.UtcNow },

            new() { UnitId = u1.Id,
                    Title = "🔴 Winters, Manshreck & Wright — Software Engineering at Google (O'Reilly 2020) · Cap. 1",
                    Description = "OBLIGATORIO · Tema 1. Lecciones aprendidas programando a escala en Google a lo largo del tiempo.",
                    Type = ResourceType.Link, Url = "https://abseil.io/resources/swe-book",
                    UploadedById = profesor.Id, CreatedAt = DateTime.UtcNow },

            new() { UnitId = u1.Id,
                    Title = "🔵 OReilly — Software Engineering at Google 2020 (PDF)",
                    Description = "COMPLEMENTARIO · Tema 1. PDF descargable del libro Software Engineering at Google — O'Reilly Media.",
                    Type = ResourceType.Link, Url = "https://abseil.io/resources/swe-book/html/toc.html",
                    UploadedById = profesor.Id, CreatedAt = DateTime.UtcNow },

            // ── Tema 2: Estado Actual y Antecedentes. La Crisis del Software ───────
            new() { UnitId = u1.Id,
                    Title = "🔵 Chapter 1 Issues — The Software Crisis / Software's Chronic Crisis",
                    Description = "COMPLEMENTARIO · Tema 2. Lectura sobre los problemas históricos y crónicos del desarrollo de software.",
                    Type = ResourceType.Link, Url = "https://www.scientificamerican.com/article/software-s-chronic-crisis/",
                    UploadedById = profesor.Id, CreatedAt = DateTime.UtcNow },

            new() { UnitId = u1.Id,
                    Title = "🔵 Barry Boehm — A View of 20th and 21st Century Software Engineering",
                    Description = "COMPLEMENTARIO · Tema 2. Perspectiva histórica y prospectiva de la IS. Referente fundamental de la disciplina.",
                    Type = ResourceType.Link, Url = "https://dl.acm.org/doi/10.1145/1134285.1134288",
                    UploadedById = profesor.Id, CreatedAt = DateTime.UtcNow },

            // ── Temas 3–8: Disciplinas, Ciclos de vida, Procesos, Componentes ──────
            new() { UnitId = u1.Id,
                    Title = "🔴 Pressman Cap. 2 — Procesos Empíricos vs. Definidos y Ciclos de Vida",
                    Description = "OBLIGATORIO · Temas 3–6. Disciplinas de la IS, modelos de proceso, ventajas/desventajas de ciclos de vida y criterios de elección.",
                    Type = ResourceType.Link, Url = "https://www.mheducation.com",
                    UploadedById = profesor.Id, CreatedAt = DateTime.UtcNow },

            new() { UnitId = u1.Id,
                    Title = "🔴 Pressman Cap. 24 — Vínculo Proceso-Proyecto-Producto",
                    Description = "OBLIGATORIO · Tema 8. Gestión de un proyecto de desarrollo de software y el vínculo entre proceso, proyecto y producto.",
                    Type = ResourceType.Link, Url = "https://www.mheducation.com",
                    UploadedById = profesor.Id, CreatedAt = DateTime.UtcNow },

            // ── Material extra ────────────────────────────────────────────────────
            new() { UnitId = u1.Id,
                    Title = "🔵 Booch — The History of Software Engineering (IEEE Software 2018)",
                    Description = "COMPLEMENTARIO · Material extra. Grady Booch repasa la historia de la IS. IEEE Software, vol. 35, n.° 5, pp. 108–114.",
                    Type = ResourceType.Link, Url = "https://ieeexplore.ieee.org/document/8474489",
                    UploadedById = profesor.Id, CreatedAt = DateTime.UtcNow },
        };
        db.Resources.AddRange(recursos);

        db.SaveChanges();
    }
}
