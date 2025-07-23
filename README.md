# SMS Forwarder v1.5.0

Una aplicación Android desarrollada con .NET MAUI que permite reenviar automáticamente mensajes SMS a números configurados.

## 🚀 Características

- **Reenvío automático de SMS**: Reenvía todos los SMS recibidos a números preconfigurados
- **Gestión completa de permisos**: Sistema inteligente para configurar permisos de batería y autostart
- **Soporte multi-fabricante**: Configuración específica para Xiaomi, Huawei, OPPO, Vivo, Samsung, OnePlus
- **Interfaz moderna**: Diseño limpio y fácil de usar
- **Diagnósticos integrados**: Herramientas para verificar el funcionamiento
- **Funcionamiento en segundo plano**: Optimizado para trabajar sin interrupciones

## 📱 Requisitos

- Android 5.0 (API 21) o superior
- Permisos SMS (se solicitan automáticamente)
- Recomendado: Desactivar optimización de batería
- Recomendado: Activar autostart (según fabricante)

## 🔧 Instalación

1. Descarga el APK desde la carpeta `Release/`
2. Instala la aplicación
3. Concede los permisos SMS cuando se soliciten
4. Configura los permisos de batería y autostart desde la aplicación

## 📋 Uso

### Configuración básica
1. Abre la aplicación
2. Añade los números de teléfono donde quieres reenviar los SMS
3. Los mensajes se reenviarán automáticamente

### Configuración de permisos
1. Ve a "Permisos y Configuración"
2. Usa "Verificar Estado de Permisos" para ver el estado actual
3. Usa "Configurar Todos los Permisos" para setup automático
4. O configura individualmente "Batería" y "Autostart"

## 🏗️ Desarrollo

### Tecnologías utilizadas
- **.NET MAUI**: Framework multiplataforma
- **C#**: Lenguaje de programación
- **XAML**: Interfaz de usuario
- **Android SDK**: APIs específicas de Android

### Estructura del proyecto
```
SMSForwarder/
├── Services/               # Servicios (PermissionService, LoggingService)
├── Platforms/Android/      # Código específico de Android
├── Resources/              # Recursos (iconos, imágenes)
├── *.xaml                  # Páginas de interfaz de usuario
├── *.cs                    # Código C#
└── Release/                # APKs compilados
```

### Compilar el proyecto
```bash
# Compilar en modo Release
dotnet build -c Release

# Generar APK
dotnet publish -f net9.0-android -c Release
```

## 📝 Historial de versiones

### v1.5.0 (Actual)
- ✅ Sistema completo de gestión de permisos
- ✅ Configuración de optimización de batería
- ✅ Soporte para autostart por fabricante
- ✅ Interfaz mejorada con controles de permisos
- ✅ Solicitud automática de permisos al inicio

### v1.4.0
- ✅ Cambio de marca a "sOCratic"
- ✅ Corrección del receptor SMS
- ✅ Solución de problemas de instalación
- ✅ Mejoras en el AndroidManifest

### v1.0.0
- ✅ Funcionalidad básica de reenvío SMS
- ✅ Interfaz de configuración
- ✅ Diagnósticos básicos

## 🔒 Permisos

La aplicación requiere los siguientes permisos:

### Permisos SMS (Obligatorios)
- `RECEIVE_SMS`: Para recibir mensajes SMS
- `SEND_SMS`: Para reenviar mensajes SMS
- `READ_SMS`: Para leer el contenido de los mensajes

### Permisos del sistema (Recomendados)
- `REQUEST_IGNORE_BATTERY_OPTIMIZATIONS`: Para funcionar en segundo plano
- `RECEIVE_BOOT_COMPLETED`: Para iniciarse después del reinicio
- `WAKE_LOCK`: Para mantener el dispositivo activo cuando sea necesario

## 🛠️ Configuración por fabricante

### Xiaomi (MIUI)
- Configuración > Aplicaciones > Administrar aplicaciones > SMS Forwarder > Inicio automático

### Huawei (EMUI)
- Configuración > Aplicaciones > SMS Forwarder > Inicio automático

### OPPO (ColorOS)
- Configuración > Aplicaciones > SMS Forwarder > Permisos > Inicio automático

### Vivo (FuntouchOS)
- Configuración > Aplicaciones > SMS Forwarder > Permisos > Inicio automático

### Samsung (One UI)
- Configuración > Aplicaciones > SMS Forwarder > Batería > Optimizar uso de batería

### OnePlus (OxygenOS)
- Configuración > Aplicaciones > SMS Forwarder > Permisos > Inicio automático

## 🐛 Solución de problemas

### La aplicación no reenvía mensajes
1. Verifica que los permisos SMS estén concedidos
2. Desactiva la optimización de batería
3. Activa el autostart según tu fabricante
4. Reinicia la aplicación

### La aplicación se cierra en segundo plano
1. Desactiva la optimización de batería
2. Añade la aplicación a la lista blanca de autostart
3. Verifica que no esté en modo de ahorro de batería extremo

## 📞 Soporte

Para reportar problemas o sugerir mejoras, contacta al desarrollador.

## 📄 Licencia

© 2025 sOCratic - Todos los derechos reservados

---

**Desarrollado con ❤️ por sOCratic**