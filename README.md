# 📱 SMSForwarder

Una aplicación Android desarrollada en .NET MAUI que reenvía automáticamente los SMS recibidos a números de teléfono configurados.

## ✨ Características

### 🔄 Reenvío Automático
- **Reenvío instantáneo** de todos los SMS recibidos
- **Múltiples destinatarios** configurables
- **Formato identificable** con prefijo `[SMSForwarder]`

### 📝 Gestión de Números
- **Entrada manual** de números de teléfono
- **Selección desde contactos** con interfaz de búsqueda
- **Validación automática** de formato de números
- **Eliminación fácil** por deslizamiento

### 🛡️ Prevención de Bucles Infinitos
- **Detección inteligente** de mensajes reenviados
- **Verificación de remitente** contra lista de destinatarios
- **Prevención de duplicados** en períodos cortos
- **Logs detallados** para depuración

### 🎨 Interfaz Moderna
- **Diseño Material Design** con iconos intuitivos
- **Tema claro/oscuro** automático
- **Navegación fluida** entre secciones
- **Feedback visual** para todas las acciones

## 🚀 Instalación

### Requisitos
- Android 7.0 (API 24) o superior
- Permisos de SMS y Contactos

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
2. **Concede permisos** de SMS y Contactos cuando se soliciten
3. **Agrega números** de destino usando una de estas opciones:
   - Escribir manualmente en el campo de texto
   - Seleccionar desde contactos con el botón "👥 Desde Contactos"

### Gestión de Números
- **Agregar**: Usa el botón "📝 Agregar Número" o "👥 Desde Contactos"
- **Eliminar**: Desliza hacia la izquierda en cualquier número de la lista
- **Validación**: Los números se validan automáticamente al agregarlos

### Diagnósticos
- Ve a la sección **"Diagnósticos"** para:
  - Verificar permisos del sistema
  - Probar envío de SMS
  - Ver logs de actividad
  - Configurar inicio automático

## 🔧 Configuración Avanzada

### Permisos Requeridos
- `RECEIVE_SMS` - Para recibir mensajes entrantes
- `SEND_SMS` - Para reenviar mensajes
- `READ_CONTACTS` - Para seleccionar desde contactos

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

### Problemas de contactos
1. Verifica el permiso de lectura de contactos
2. Asegúrate de tener contactos con números de teléfono
3. Reinicia la aplicación si es necesario

## 📋 Roadmap

### Próximas Características
- [ ] **Filtros de mensajes** por remitente o contenido
- [ ] **Programación de horarios** para reenvío
- [ ] **Estadísticas de uso** y reportes
- [ ] **Backup y restauración** de configuración
- [ ] **Soporte para MMS** (mensajes multimedia)

### Mejoras Técnicas
- [ ] **Migración a CommunityToolkit.Mvvm** para messaging
- [ ] **Optimización de rendimiento** en listas grandes
- [ ] **Soporte para temas personalizados**
- [ ] **Localización** a múltiples idiomas

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
