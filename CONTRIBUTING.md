# 🤝 Guía de Contribución

¡Gracias por tu interés en contribuir a SMSForwarder! Esta guía te ayudará a empezar.

## 📋 Tabla de Contenidos

- [Código de Conducta](#código-de-conducta)
- [¿Cómo puedo contribuir?](#cómo-puedo-contribuir)
- [Configuración del Entorno](#configuración-del-entorno)
- [Proceso de Desarrollo](#proceso-de-desarrollo)
- [Estándares de Código](#estándares-de-código)
- [Envío de Pull Requests](#envío-de-pull-requests)

## 📜 Código de Conducta

Este proyecto adhiere al código de conducta de la comunidad. Al participar, se espera que mantengas este código.

## 🚀 ¿Cómo puedo contribuir?

### 🐛 Reportar Bugs

Si encuentras un bug:

1. **Verifica** que no haya sido reportado anteriormente
2. **Crea un issue** con la etiqueta `bug`
3. **Incluye información detallada**:
   - Versión de Android
   - Pasos para reproducir
   - Comportamiento esperado vs actual
   - Screenshots si es relevante

### ✨ Sugerir Mejoras

Para sugerir nuevas características:

1. **Crea un issue** con la etiqueta `enhancement`
2. **Describe claramente** la funcionalidad propuesta
3. **Explica por qué** sería útil para los usuarios
4. **Proporciona mockups** si es aplicable

### 🔧 Contribuir Código

1. **Fork** el repositorio
2. **Crea una rama** para tu feature
3. **Implementa** los cambios
4. **Escribe tests** si es aplicable
5. **Envía un Pull Request**

## 🛠️ Configuración del Entorno

### Requisitos

- **Visual Studio 2022** o **Visual Studio Code**
- **.NET 9.0 SDK**
- **Android SDK** (API 24+)
- **Git**

### Instalación

1. Clona el repositorio:
   ```bash
   git clone https://github.com/tu-usuario/SMSForwarder.git
   cd SMSForwarder
   ```

2. Restaura dependencias:
   ```bash
   dotnet restore
   ```

3. Compila el proyecto:
   ```bash
   dotnet build
   ```

## 🔄 Proceso de Desarrollo

### Flujo de Trabajo

1. **Asigna** o crea un issue
2. **Crea una rama** desde `main`:
   ```bash
   git checkout -b feature/nombre-descriptivo
   ```
3. **Desarrolla** la funcionalidad
4. **Commit** frecuentemente con mensajes descriptivos
5. **Push** a tu fork
6. **Crea un Pull Request**

### Convenciones de Ramas

- `feature/` - Nuevas funcionalidades
- `bugfix/` - Corrección de bugs
- `hotfix/` - Correcciones urgentes
- `docs/` - Cambios en documentación

### Mensajes de Commit

Usa el formato:
```
tipo(alcance): descripción breve

Descripción más detallada si es necesario.

Fixes #123
```

Tipos:
- `feat` - Nueva funcionalidad
- `fix` - Corrección de bug
- `docs` - Documentación
- `style` - Formato de código
- `refactor` - Refactorización
- `test` - Tests
- `chore` - Tareas de mantenimiento

## 📝 Estándares de Código

### C# / .NET

- **Sigue las convenciones** de C# de Microsoft
- **Usa PascalCase** para métodos y propiedades públicas
- **Usa camelCase** para variables locales
- **Incluye documentación XML** para APIs públicas
- **Maneja excepciones** apropiadamente

### XAML

- **Usa nombres descriptivos** para elementos
- **Organiza** los recursos lógicamente
- **Sigue el patrón MVVM** cuando sea aplicable

### Ejemplo de Código

```csharp
/// <summary>
/// Valida un número de teléfono
/// </summary>
/// <param name="phoneNumber">El número a validar</param>
/// <returns>True si es válido, false en caso contrario</returns>
public bool ValidatePhoneNumber(string phoneNumber)
{
    if (string.IsNullOrWhiteSpace(phoneNumber))
        return false;
        
    // Lógica de validación
    return phoneRegex.IsMatch(phoneNumber);
}
```

## 📤 Envío de Pull Requests

### Antes de Enviar

- [ ] **Compila** sin errores
- [ ] **Pasan todos los tests**
- [ ] **Sigue** los estándares de código
- [ ] **Actualiza** documentación si es necesario
- [ ] **Incluye tests** para nuevas funcionalidades

### Template de PR

```markdown
## 📋 Descripción

Breve descripción de los cambios realizados.

## 🔗 Issue Relacionado

Fixes #(número del issue)

## 🧪 Tipo de Cambio

- [ ] Bug fix (cambio que corrige un issue)
- [ ] Nueva funcionalidad (cambio que agrega funcionalidad)
- [ ] Breaking change (cambio que rompe compatibilidad)
- [ ] Documentación

## ✅ Checklist

- [ ] Mi código sigue los estándares del proyecto
- [ ] He realizado una auto-revisión de mi código
- [ ] He comentado mi código en áreas complejas
- [ ] He actualizado la documentación correspondiente
- [ ] Mis cambios no generan nuevas advertencias
- [ ] He agregado tests que prueban mi fix/feature
- [ ] Los tests nuevos y existentes pasan localmente
```

## 🧪 Testing

### Ejecutar Tests

```bash
dotnet test
```

### Escribir Tests

- **Usa nombres descriptivos** para los tests
- **Sigue el patrón AAA** (Arrange, Act, Assert)
- **Cubre casos edge** y escenarios de error

## 📚 Documentación

### Actualizar Documentación

- **README.md** - Para cambios en funcionalidades principales
- **CHANGELOG.md** - Para todos los cambios
- **Comentarios de código** - Para lógica compleja

## ❓ ¿Necesitas Ayuda?

- **Crea un issue** con la etiqueta `question`
- **Revisa issues existentes** con `help wanted`
- **Contacta** a los mantenedores

## 🎉 Reconocimiento

Todos los contribuidores serán reconocidos en el README del proyecto.

---

¡Gracias por contribuir a SMSForwarder! 🚀