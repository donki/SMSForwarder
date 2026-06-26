# Changelog

Todos los cambios notables de este proyecto serán documentados en este archivo.

El formato está basado en [Keep a Changelog](https://keepachangelog.com/es-ES/1.0.0/),
y este proyecto adhiere al [Versionado Semántico](https://semver.org/lang/es/).

## [2026.06.26.0] - 2026-06-26

### ✨ Agregado
- **Soporte multiidioma**: Localización completa en Español e Inglés
- **Detección automática de idioma**: La aplicación detecta el idioma del dispositivo automáticamente
- **Fallback a inglés**: Si el dispositivo no está en español, se muestra en inglés por defecto
- **Servicio de localización**: ILocalizationService para gestionar strings localizados
- **Strings localizados**: Todos los textos de la interfaz traducidos

### 🐛 Correcciones
- Actualización de versión a 2026.06.26.0

## [1.10.0] - 2024-12-19

### ✨ Agregado
- **Selección desde contactos**: Nueva funcionalidad para seleccionar números desde la lista de contactos
- **Interfaz de búsqueda de contactos**: Búsqueda y filtrado en tiempo real
- **Sistema dual de comunicación**: Eventos + MessagingCenter para mayor robustez
- **Prevención de bucles infinitos**: Sistema completo de detección y prevención
- **Logs de depuración**: Sistema de logging detallado para troubleshooting
- **Validación mejorada**: Comparación inteligente de números de teléfono
- **Formato identificable**: Mensajes reenviados con prefijo `[SMSForwarder]`

### 🛡️ Seguridad
- **Detección de bucles**: Previene reenvíos infinitos entre dispositivos
- **Validación de remitente**: Verifica si el mensaje proviene de un número en la lista
- **Detección de duplicados**: Evita reenvíos múltiples del mismo mensaje

### 🎨 Interfaz
- **Botones con iconos**: "📝 Agregar Número" y "👥 Desde Contactos"
- **Diseño moderno**: Interfaz actualizada con Material Design
- **Feedback visual**: Confirmaciones y mensajes de error mejorados

### 🔧 Técnico
- **Arquitectura mejorada**: Servicios con inyección de dependencias
- **Manejo de permisos**: Gestión automática de permisos de contactos
- **Compatibilidad**: Soporte para Android 7.0+ (API 24)

## [1.7.0] - 2024-12-18

### ✨ Agregado
- **Reenvío automático de SMS**: Funcionalidad principal implementada
- **Gestión de números**: Agregar y eliminar números de destino
- **Validación de números**: Verificación de formato de números telefónicos
- **Interfaz de usuario**: Diseño inicial con navegación por pestañas
- **Página de diagnósticos**: Herramientas para verificar permisos y funcionalidad
- **Persistencia de datos**: Almacenamiento local de configuración

### 🔧 Técnico
- **Base .NET MAUI**: Migración completa a .NET MAUI
- **Arquitectura MVVM**: Implementación de patrón MVVM
- **Servicios asíncronos**: Operaciones no bloqueantes
- **Manejo de permisos**: Gestión de permisos de SMS

### 📱 Android
- **BroadcastReceiver**: Recepción de SMS en tiempo real
- **Permisos nativos**: Integración con sistema de permisos Android
- **Optimización de batería**: Configuración para funcionamiento en segundo plano

## [1.0.0] - 2024-12-15

### ✨ Agregado
- **Proyecto inicial**: Configuración base del proyecto
- **Estructura básica**: Organización de carpetas y archivos
- **Configuración MAUI**: Setup inicial de .NET MAUI
- **Iconos y recursos**: Recursos gráficos básicos

---

## Tipos de Cambios

- `✨ Agregado` para nuevas funcionalidades
- `🔧 Cambiado` para cambios en funcionalidades existentes
- `❌ Obsoleto` para funcionalidades que serán removidas
- `🗑️ Removido` para funcionalidades removidas
- `🐛 Corregido` para corrección de bugs
- `🛡️ Seguridad` para vulnerabilidades corregidas