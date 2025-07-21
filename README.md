SMS Forwarder
Aplicación Android para reenvío automático de mensajes SMS con interfaz moderna y profesional.

📱 Descripción

SMS Forwarder es una aplicación Android nativa desarrollada en .NET MAUI que permite reenviar automáticamente mensajes SMS entrantes a números de teléfono preconfigurados. Ideal para mantener el control de comunicaciones importantes y asegurar que los mensajes lleguen a múltiples destinatarios.

✨ Características principales

🔄 Reenvío automático de SMS a números configurados

⚙️ Configuración simple e intuitiva de números de destino

🔍 Diagnósticos integrados para monitoreo del sistema

📱 Interfaz moderna con diseño Material Design

🎨 Splash screen personalizado con animaciones suaves

🧭 Navegación intuitiva con menú lateral

🛡️ Gestión de permisos SMS integrada

📊 Sistema de logging para seguimiento de actividad

🎯 Funcionalidades técnicas

Interceptación SMS: Captura automática de mensajes entrantes

Reenvío inteligente: Envío a múltiples destinatarios configurados

Persistencia: Almacenamiento seguro de configuraciones

Diagnósticos: Herramientas de monitoreo y troubleshooting

UI/UX profesional: Títulos unificados y diseño consistente

Safe Area: Respeto por las áreas seguras del dispositivo

🔧 Requisitos técnicos

Android: 5.0+ (API 21)

Permisos: SMS (RECEIVE_SMS, SEND_SMS, READ_SMS)

Arquitectura: ARM64, ARM, x86, x64

# Compilar para Android
dotnet build --framework net9.0-android --configuration Release

# Generar APK
dotnet publish --framework net9.0-android --configuration Release
🎨 Capturas de pantalla
| Splash Screen | Configuración | Diagnósticos | Acerca de | |---------------|---------------|--------------|-----------| |||||

🏗️ Arquitectura técnica

Framework: .NET MAUI 9.0

Lenguaje: C# 12

UI: XAML con Material Design

Plataformas: Android, Windows

Patrón: MVVM con Code-behind

Persistencia: Preferences API

🔐 Permisos requeridos

<uses-permission android:name="android.permission.RECEIVE_SMS" />

<uses-permission android:name="android.permission.SEND_SMS" />

<uses-permission android:name="android.permission.READ_SMS" />

<uses-permission android:name="android.permission.BROADCAST_SMS" />

🚀 Uso

Configurar números: Agregar números de destino en la pantalla principal

Verificar permisos: Asegurar que los permisos SMS estén concedidos

Activar servicio: La app interceptará SMS automáticamente

Monitorear: Usar diagnósticos para verificar el funcionamiento

🛠️ Desarrollo

Estructura del proyecto

SMSForwarder/

├── Platforms/Android/     # Código específico Android

├── Resources/             # Recursos (iconos, splash, etc.)

├── Services/              # Servicios de logging

├── *.xaml                # Páginas de la interfaz

├── *.xaml.cs             # Code-behind

└── SMSForwarder.csproj   # Configuración del proyecto

Tecnologías utilizadas

.NET MAUI: Framework multiplataforma

XAML: Definición de interfaz

Android APIs: Gestión de SMS

Material Design: Principios de diseño

Dependency Injection: Inyección de dependencias

📄 Licencia
Este proyecto está bajo la Licencia MIT - ver el archivo LICENSE para detalles.
