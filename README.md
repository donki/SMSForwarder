# SMSForwarder

Una aplicación Android de mensajes desarrollada en .NET MAUI: gestiona tus SMS y, además, reenvía automáticamente los que recibes a los números de teléfono que configures.

## Dónde conseguirla

- **Google Play:** https://play.google.com/store/apps/details?id=com.socratic.smsforwarder
- **Releases de GitHub** (APK / EXE / MSIX de cada versión): https://github.com/donki/SMSForwarder/releases

## ✨ Características

### 💬 Mensajes
- **Buzón** de mensajes recibidos y enviados
- **Redacción y envío** de SMS, con selector de contactos del sistema
- **Ver el mensaje completo** tocándolo en el buzón (se marca como leído), con responder, reenviar, copiar el número o el texto y abrir enlaces
- **Borrar** con el botón de papelera de cada fila, o varios a la vez con la selección múltiple (pulsación larga)
- **App de SMS predeterminada**: funciona como gestor de mensajes del teléfono

### 🔄 Reenvío Automático
- **Reenvío instantáneo** de los SMS recibidos
- **Múltiples destinatarios** configurables
- **Filtros por destinatario**: tocando un número eliges qué SMS le llegan (todos, solo de ciertos remitentes o solo los que contengan ciertas palabras)
- **Formato identificable** con prefijo `[SMSForwarder]`

### 📝 Gestión de Números
- **Entrada manual** de números de teléfono
- **Selección desde contactos** con el selector del sistema (sin permiso de Contactos)
- **Validación automática** de formato de números
- **Eliminación** con el botón de papelera de cada número

### 🛡️ Prevención de Bucles Infinitos
- **Detección inteligente** de mensajes reenviados
- **Verificación de remitente** contra lista de destinatarios
- **Prevención de duplicados** en períodos cortos
- **Logs detallados** para depuración

### 🎨 Interfaz Moderna
- **Diseño Material Design** con iconos intuitivos
- **Tema claro/oscuro** automático
- **Castellano e inglés**: sigue el idioma del teléfono y se puede cambiar en Configuración
- **Navegación fluida** entre secciones
- **Feedback visual** para todas las acciones

## 🚀 Instalación

### Requisitos
- Android 5.0 (API 21) o superior; compilada contra la API 36
- Permiso de SMS (el selector de contactos es el del sistema, no requiere permiso)

### Desde Código Fuente
1. Clona el repositorio:
   ```bash
   git clone https://github.com/tu-usuario/SMSForwarder.git
   cd SMSForwarder
   ```

2. Restaura las dependencias:
   ```bash
   dotnet restore
   ```

3. Compila y ejecuta:
   ```bash
   dotnet build -t:Run -f net9.0-android
   ```

## 📖 Uso

### Configuración Inicial
1. **Abre la aplicación** y ve a la sección "Configuración"
2. **Acepta ser la app de SMS predeterminada** (sale lo primero) y concede el permiso de SMS y el de notificaciones. La app no pide acceso a Contactos: el botón «Contactos» abre el selector del sistema, que solo le pasa el número elegido
3. **Agrega números** de destino usando una de estas opciones:
   - Escribir manualmente en el campo de texto
   - Seleccionar desde contactos con el botón «Contactos»

### Gestión de Números
- **Agregar**: Usa el botón «Agregar número» o «Contactos»
- **Elegir qué SMS recibe**: toca el número
- **Eliminar**: toca la papelera del número y confirma
- **Validación**: Los números se validan automáticamente al agregarlos

### Diagnósticos
- Ve a la sección **"Diagnósticos"** para:
  - Ver el estado de los permisos (en castellano o en inglés, según el idioma de la app)
  - Configurar la batería y el inicio automático
  - Ver y limpiar el registro de actividad

## 🔧 Configuración Avanzada

### Permisos Requeridos
- `RECEIVE_SMS` - Para recibir mensajes entrantes
- `SEND_SMS` - Para enviar y reenviar mensajes
- `READ_SMS` - Para listar el buzón del teléfono en la pantalla Mensajes
- `RECEIVE_BOOT_COMPLETED` - Para seguir reenviando tras reiniciar el teléfono
- `POST_NOTIFICATIONS` - Para avisar de los SMS entrantes (Android 13+)

**Sin `READ_CONTACTS` a propósito**: elegir un número abre el selector del sistema
(`ACTION_PICK`), que devuelve solo el número escogido. Lo exige la política de Google Play
sobre acceso amplio a contactos (obligatoria el 2026-10-28).

### Optimización de Batería
La aplicación puede requerir **exclusión de optimización de batería** para funcionar correctamente en segundo plano. Esto se configura automáticamente desde la sección de Diagnósticos.

## 🛠️ Desarrollo

### Tecnologías Utilizadas
- **.NET 9.0** - Framework principal
- **.NET MAUI** - UI multiplataforma
- **C#** - Lenguaje de programación
- **Android SDK** - APIs nativas de Android

### Estructura del Proyecto
```
SMSForwarder/
├── Models/                 # Modelos de datos
├── Services/              # Servicios de negocio
├── Platforms/Android/     # Código específico de Android
├── Resources/             # Recursos (iconos, estilos, etc.)
├── Pages/                 # Páginas de la aplicación
└── App.xaml              # Configuración de la aplicación
```

### Características Técnicas
- **Arquitectura MVVM** con inyección de dependencias
- **Servicios asíncronos** para operaciones de red
- **Logging integrado** para depuración
- **Manejo robusto de errores** y excepciones

### Pruebas

**90 pruebas** (xUnit), todas pasan · cobertura del código probado **99,3 %** de líneas · sobre
toda la app **16,9 %** (654 de ~3 870 líneas; el resto es interfaz MAUI y código de Android:
receptores de SMS, buzón del sistema, permisos) · el banco tarda **~0,2 s** (≈ 6 s con la
cobertura). Medido el 2026-09-30.

```powershell
dotnet test SMSForwarder.Tests
# con cobertura (coverlet) y resumen (ReportGenerator, herramienta local del repo)
dotnet test SMSForwarder.Tests -s SMSForwarder.Tests/coverlet.runsettings --collect:"XPlat Code Coverage"
dotnet tool restore; dotnet tool run reportgenerator -reports:SMSForwarder.Tests/TestResults/*/coverage.cobertura.xml -targetdir:SMSForwarder.Tests/TestResults/report -reporttypes:TextSummary
```

Se prueban los filtros por número y por palabra (sin mayúsculas ni acentos), la comparación de
números con y sin prefijo, la validación al añadir, el formato y recorte del reenvío, la detección
de bucles y de duplicados, el guardado de destinos (formato nuevo y viejo), los idiomas (mismas
claves y huecos en los dos) y el registro. Sin red, sin SMS y sin dispositivo.

## 🔒 Seguridad y Privacidad

### Datos Locales
- Los números de teléfono se almacenan **localmente** en el dispositivo
- **No se envían datos** a servidores externos
- **Cifrado automático** por el sistema Android

### Permisos Mínimos
- Solo solicita permisos **estrictamente necesarios**
- **Transparencia total** sobre el uso de permisos
- **Control completo** del usuario sobre los datos

## 🐛 Solución de Problemas

### Los SMS no se reenvían
1. Verifica que la aplicación tenga todos los permisos necesarios
2. Asegúrate de que esté excluida de la optimización de batería
3. Revisa que los números estén correctamente configurados

### Bucles infinitos
La aplicación incluye **protección automática** contra bucles:
- Detecta mensajes que provienen de números en la lista de reenvío
- Identifica mensajes ya reenviados por el formato `[SMSForwarder]`
- Previene duplicados en períodos cortos

### No puedo borrar mensajes del buzón
Android solo permite borrar y marcar como leídos a la **app de SMS predeterminada**. Abre la
pantalla Mensajes y pulsa "Usar como predeterminada".

## 📋 Roadmap

### Próximas Características
- [ ] **Programación de horarios** para reenvío
- [ ] **Estadísticas de uso** y reportes
- [ ] **Backup y restauración** de configuración
- [ ] **Soporte para MMS** (mensajes multimedia)

### Mejoras Técnicas
- [ ] **Migración a CommunityToolkit.Mvvm** para messaging
- [ ] **Optimización de rendimiento** en listas grandes
- [ ] **Soporte para temas personalizados**

## 🤝 Contribuir

¡Las contribuciones son bienvenidas! Por favor:

1. **Fork** el proyecto
2. Crea una **rama para tu feature** (`git checkout -b feature/AmazingFeature`)
3. **Commit** tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. **Push** a la rama (`git push origin feature/AmazingFeature`)
5. Abre un **Pull Request**

### Guías de Contribución
- Sigue las convenciones de código existentes
- Incluye tests para nuevas funcionalidades
- Actualiza la documentación según sea necesario
- Asegúrate de que todos los tests pasen

## 📄 Licencia

Este proyecto está licenciado bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para más detalles.

## 👨‍💻 Autor

Desarrollado con ❤️ para la comunidad Android.

## 🙏 Agradecimientos

- **Microsoft** por .NET MAUI
- **Comunidad .NET** por las librerías y herramientas
- **Contribuidores** que hacen posible este proyecto

---

⭐ **¡Si te gusta este proyecto, dale una estrella en GitHub!** ⭐
