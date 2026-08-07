# Changelog

Todos los cambios notables de este proyecto serán documentados en este archivo.

El formato está basado en [Keep a Changelog](https://keepachangelog.com/es-ES/1.0.0/),
y este proyecto adhiere al [Versionado Semántico](https://semver.org/lang/es/).

## [2026.08.07.0] - 2026-08-07

### 📋 Cumplimiento de políticas
- **Política de privacidad propia** ([`PRIVACY.md`](PRIVACY.md)): detalla el tratamiento de datos
  con el enfoque de app de SMS predeterminada, permiso a permiso, y deja explícito que el reenvío
  viaja por la red del operador sin servidor intermedio. Es coherente con declarar "no se recopilan
  datos" en la sección Seguridad de los datos de Play

### 🔧 Cambiado
- Versión regenerada a fecha de hoy (`202608070`). El binario `202608020` quedó atrapado en el
  canal *internal* mientras Google revisaba `202606260` en Alpha y `202607280` en producción, que
  no incluyen `SmsDeliverReceiver` ni `MmsDeliverReceiver`: de ahí el rechazo del 2026-08-06 por
  *"does not appear to have default handler capability"*. Sin cambios funcionales respecto a
  `202608020`

## [2026.08.02.0] - 2026-08-02

### ✨ Agregado
- **Buzón de mensajes** (`MessagesPage`): lista de SMS recibidos y enviados leídos del proveedor
  del sistema, con marcar como leído, borrar deslizando y responder tocando el mensaje
- **Redacción de mensajes** (`ComposePage`): destinatario con selector de contactos, contador de
  caracteres y partes del SMS, y envío real guardado en Enviados
- **Petición del rol de app de SMS predeterminada** desde el propio buzón

### 🔧 Cambiado
- `ComposeSmsActivity` deja de ser un redirector vacío: extrae destinatario y texto de los intents
  `sms:`/`smsto:` (`sms_body`, `EXTRA_TEXT`, parámetro `body`) y abre la pantalla de redacción
- `HeadlessSmsSendService` deja de ser un no-op: envía de verdad la respuesta rápida a una llamada
  entrante y la guarda en Enviados
- "Mensajes" pasa a ser la primera entrada del menú

### 🐛 Correcciones
- **Detección de "app de SMS predeterminada" en Xiaomi/HyperOS**: se consulta `RoleManager`
  (fuente de verdad desde Android 10) en lugar de `Telephony.Sms.GetDefaultSmsPackage`. Estos
  dispositivos conceden el rol dejando `Settings.Secure.sms_default_application` a null, con lo
  que la app se creía no predeterminada: mostraba el aviso de forma permanente, bloqueaba el
  borrado de mensajes y no inhibía el camino `SMS_RECEIVED` (doble reenvío)
- La carpeta `_RECUPERADO_DEL_DLL` (volcado decompilado de referencia) se excluye de la
  compilación: duplicaba tipos y rompía el build

### 📋 Cumplimiento de políticas
- Los cuatro componentes obligatorios de app de SMS por defecto están implementados, no declarados
  en vacío. Es el requisito de la política de Permisos SMS/Call Log de Google Play, que prohíbe
  pedir permisos sensibles para funciones no implementadas (rechazo del 2026-08-02, routing ZLFS)
- **Ficha de Play reescrita**: `PlayStoreListing.es-ES.json` seguía con el texto centrado en el
  reenvío, que es el caso de uso prohibido. Ahora presenta la app como gestor de SMS
  predeterminado, tal y como exige la política de promocionar la función que justifica el permiso
- **Permisos de la ficha alineados con el manifest**: `ficha.md` declaraba `READ_CONTACTS` e
  `INTERNET`, que la app no pide. Declarar permisos que el APK no usa es motivo de rechazo

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