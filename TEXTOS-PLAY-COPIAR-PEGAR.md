# Textos para Play Console — SMS Forwarder 2026.08.02.0

Todo lo que hay que copiar y pegar en Play Console para el reenvío a revisión tras el rechazo del
2026-08-02 (*Ineligible use case*, routing ID `ZLFS`). Contexto y motivo de cada decisión en
[PLAY-DECLARACION-SMS.md](PLAY-DECLARACION-SMS.md).

**Datos de la versión:** `com.socratic.smsforwarder` · versión `2026.08.02.0` · versionCode `202608020`

> Los bloques ``` son para copiar tal cual. Los contadores de caracteres están comprobados contra
> los límites de Play.

---

# 1. Declaración de permisos de SMS y registro de llamadas

**Ruta:** Play Console → Política → Contenido de la aplicación → Permisos de SMS y registro de llamadas

### 1.1 Permisos a declarar

Marca **exactamente** estos tres, ni uno más:

```
RECEIVE_SMS
SEND_SMS
READ_SMS
```

> El commit por API falla con *"permissions missing from declaration: READ_SMS"* si se olvida el tercero.

### 1.2 Caso de uso principal

Selecciona **solo** este:

```
Default SMS handler
```

**NO marques**, bajo ningún concepto:
- *Initiate a text message* ← imputado en el rechazo anterior
- *Initiate a phone call (Fast Dial)* ← imputado en el rechazo anterior
- Cualquier otro de la lista de usos no permitidos

Ambos tienen alternativa sin permiso (SMS Intent y Dial Intent), así que Google nunca concede el
permiso para ellos. Es literalmente lo que nos rechazaron.

### 1.3 Justificación del uso (campo de texto libre)

```
SMS Forwarder es una aplicación de mensajes SMS que funciona como gestor de SMS predeterminado del teléfono (default SMS handler). Implementa los cuatro componentes que Android exige para ese rol, y los cuatro tienen funcionalidad real:

1. SMS_DELIVER: recibe los mensajes entrantes, los persiste en el proveedor del sistema y notifica al usuario.
2. WAP_PUSH_DELIVER: componente requerido por el rol. La aplicación no gestiona MMS.
3. SENDTO / VIEW / SEND sobre los esquemas sms: y smsto:: abre la pantalla de redacción con el destinatario y el texto que envía la aplicación que invoca el intent.
4. RESPOND_VIA_MESSAGE: envía la respuesta rápida por SMS a una llamada entrante y la guarda en Enviados.

Uso de cada permiso:

- READ_SMS: mostrar el buzón de mensajes recibidos y enviados en la pantalla "Mensajes", que es la pantalla principal de la aplicación. Sin este permiso la aplicación no puede mostrar los mensajes del usuario.
- RECEIVE_SMS: recibir los mensajes entrantes para mostrarlos y notificarlos.
- SEND_SMS: enviar mensajes desde la pantalla de redacción, enviar la respuesta rápida a una llamada entrante, y realizar el reenvío automático que el propio usuario configura.

El reenvío automático es una función secundaria y opcional: el usuario decide si lo activa y a qué números. No es el caso de uso que justifica los permisos; el rol de gestor de SMS predeterminado sí.

Privacidad: los datos no salen del dispositivo. Los mensajes se leen del proveedor del sistema y se muestran localmente, y los números de reenvío se guardan solo en el teléfono. No hay servidores, ni analítica, ni venta ni cesión de datos a terceros.
```

### 1.4 Instrucciones para la revisión *(máx. 500 caracteres)*

Campo *Instruccions per a la revisió* / *Instructions for review*. Recomendado en **inglés**: es el
idioma en el que trabaja el equipo de revisión de Play.

**Inglés — 486/500:**

```
SMS Forwarder is a default SMS handler. To review: Android Settings > Default apps > SMS app > choose SMS Forwarder. The Messages screen lists received and sent SMS (READ_SMS); tap one to reply, Compose to send a new SMS (SEND_SMS), swipe to delete. Incoming messages arrive via SMS_DELIVER (RECEIVE_SMS). Call quick-reply uses RESPOND_VIA_MESSAGE. Automatic forwarding is an optional secondary feature the user configures. Nothing leaves the device: the app has no INTERNET permission.
```

**Español — 482/500** (sin acentos, por si el campo los cuenta como más de un carácter):

```
SMS Forwarder es un gestor de SMS predeterminado. Para revisar: Ajustes de Android > Apps predeterminadas > App de SMS > SMS Forwarder. La pantalla Mensajes lista los SMS recibidos y enviados (READ_SMS); toca uno para responder, Redactar para enviar (SEND_SMS), desliza para borrar. Los entrantes llegan por SMS_DELIVER (RECEIVE_SMS). La respuesta rapida a llamada usa RESPOND_VIA_MESSAGE. El reenvio automatico es opcional y secundario. Nada sale del dispositivo: no pide INTERNET.
```

Los dos dicen lo mismo y en el mismo orden: **cómo activar el rol** (sin eso el revisor no ve nada
de lo que justifica los permisos), **qué permiso justifica cada pantalla**, y que **el reenvío es
secundario** — que es exactamente lo que se leyó al revés en el rechazo anterior.

### 1.5 Vídeo de demostración

Súbelo a YouTube **como "no listado"** y pega el enlace. Sin cortes, 60-90 segundos:

1. Ajustes de Android → aplicación de SMS predeterminada → elegir **SMS Forwarder**.
2. Abrir la app: se ve la pantalla **Mensajes** con el buzón cargado.
3. Tocar un mensaje → se abre la respuesta con el remitente relleno → escribir y **enviar**.
4. Volver, pestaña **Enviados**: aparece el mensaje recién enviado.
5. Deslizar un mensaje → **Eliminar** → confirmar.
6. Menú → **Configuración** → añadir un número de reenvío.
7. Enviar un SMS al teléfono desde otro móvil → llega la notificación → se reenvía solo al número
   configurado (mostrar el segundo teléfono recibiéndolo).

Los pasos 1 a 5 son los importantes: demuestran el rol de gestor predeterminado. El 6 y 7 enseñan
el reenvío como lo que es, una función secundaria.

---

# 2. Ficha de Play Store — Español (es-ES)

> Estos tres textos son **idénticos** a los de `PlayStoreListing.es-ES.json`, que es lo que sube
> `publish_aab_to_play.ps1`. Si editas uno, edita el otro o la subida los sobrescribirá.

### 2.1 Nombre de la aplicación *(máx. 30 · usa 23)*

```
SMS Forwarder: Mensajes
```

El título promociona la función que justifica los permisos, que es un requisito explícito de la
política. Si prefieres no tocar el nombre, la alternativa es `SMS Forwarder`, pero entonces la
descripción corta tiene que cargar sola con ese requisito.

### 2.2 Descripción breve *(máx. 80 · usa 74)*

```
App de SMS predeterminada: envía, recibe, gestiona y reenvía tus mensajes.
```

### 2.3 Descripción completa *(máx. 4000 · usa 2142)*

```
SMS Forwarder es una aplicación de mensajes SMS completa que puedes establecer como tu app de SMS predeterminada en Android.

GESTIONA TUS SMS
• Recibe los SMS en la app y consúltalos en la pantalla Mensajes.
• Lee los mensajes recibidos y los enviados.
• Escribe y envía SMS a cualquier número o a un contacto de tu agenda.
• Responde a las conversaciones y borra los mensajes que ya no necesitas.
• Responde con un mensaje a una llamada entrante.
• Notificaciones al recibir un mensaje nuevo.

APP DE SMS PREDETERMINADA
Al abrir la aplicación te ofrecemos establecerla como tu app de SMS predeterminada. Es lo que le permite recibir, mostrar, enviar y borrar tus mensajes, además de responder llamadas con un SMS rápido. Puedes cambiar de app predeterminada cuando quieras desde los ajustes de Android.

REENVÍO AUTOMÁTICO (OPCIONAL)
Si lo activas, la aplicación puede reenviar por SMS los mensajes que recibas a uno o varios números que tú configures. Es útil, por ejemplo, para no perder los mensajes de un segundo teléfono o de una SIM que no llevas encima.
• Añade los números de destino a mano o eligiéndolos de tus contactos.
• Gestiona la lista de destinos con total libertad: puedes modificarla o vaciarla en cualquier momento.
• Detección de mensajes ya reenviados para evitar bucles.
• Pantalla de diagnóstico y registro de actividad para comprobar que todo funciona.

PRIVACIDAD
Todo el procesamiento es local en tu dispositivo. sOCratic no recibe, no almacena y no comparte copias de tus mensajes. No hay cuentas, ni anuncios, ni rastreadores.

PERMISOS
• SMS (recibir, leer y enviar): imprescindibles para funcionar como app de SMS predeterminada, es decir, para mostrarte los mensajes, enviarlos y reenviarlos si tú lo configuras.
• Notificaciones: para avisarte cuando llega un mensaje nuevo.
• La aplicación NO pide permiso de acceso a tus contactos: para elegir un destinatario se abre el selector de contactos del sistema, que solo nos devuelve el número que tú has elegido.

Usa esta aplicación únicamente con líneas telefónicas y mensajes sobre los que tengas autorización.

Software libre con licencia MIT, hecho por sOCratic.
```

---

# 3. Ficha de Play Store — Inglés (en-US)

### 3.1 App name *(máx. 30)*

```
SMS Forwarder: Messages
```

### 3.2 Short description *(máx. 80)*

```
Default SMS app: send, receive, manage and forward your text messages.
```

### 3.3 Full description *(máx. 4000)*

```
SMS Forwarder is a full SMS messaging app that you can set as your default SMS app on Android.

MANAGE YOUR TEXT MESSAGES
• Receive your SMS in the app and browse them in the Messages screen.
• Read both received and sent messages.
• Write and send SMS to any number or to a contact from your address book.
• Reply to conversations and delete the messages you no longer need.
• Reply to an incoming call with a text message.
• Notifications when a new message arrives.

DEFAULT SMS APP
When you open the app, it offers to become your default SMS app. That is what allows it to receive, display, send and delete your messages, and to answer calls with a quick text. You can switch your default app at any time from the Android settings.

AUTOMATIC FORWARDING (OPTIONAL)
If you turn it on, the app can forward the messages you receive, via SMS, to one or more numbers you configure. Handy, for example, to avoid missing messages from a second phone or a SIM you don't carry with you.
• Add destination numbers manually or pick them from your contacts.
• Manage the destination list freely: edit or clear it whenever you want.
• Detection of already-forwarded messages to avoid loops.
• A diagnostics screen and an activity log to check everything works.

PRIVACY
All processing happens locally on your device. sOCratic never receives, stores or shares copies of your messages. No accounts, no ads, no trackers.

PERMISSIONS
• SMS (receive, read and send): required to work as the default SMS app, that is, to show you your messages, send them and forward them if you configure it.
• Notifications: to alert you when a new message arrives.
• The app does NOT request access to your contacts: picking a recipient opens the system contact picker, which only returns the number you chose.

Only use this app with phone lines and messages you are authorised to handle.

Free and open source under the MIT license, made by sOCratic.
```

---

# 4. Seguridad de los datos (Data safety)

**Ruta:** Play Console → Política → Contenido de la aplicación → Seguridad de los datos

Respuestas del formulario:

| Pregunta | Respuesta |
|---|---|
| ¿Tu app recopila o comparte alguno de los tipos de datos de usuario obligatorios? | **No** |
| ¿Se cifran los datos en tránsito? | No aplica (no hay transmisión) |
| ¿Pueden los usuarios solicitar que se eliminen sus datos? | No aplica (no se recopilan) |

**Por qué "No recopila":** en la definición de Google, *recopilar* significa transmitir datos fuera
del dispositivo a los servidores del desarrollador. SMS Forwarder no tiene servidores, no declara
el permiso `INTERNET` y no hace ninguna petición de red. El reenvío por SMS lo dirige el usuario
hacia números que él mismo configura, y va por la red del operador, no a sOCratic.

Texto por si el formulario o un revisor pide aclaración:

```
SMS Forwarder no recopila ni transmite datos de usuario. La aplicación no incluye el permiso de acceso a Internet y no realiza ninguna petición de red. Los mensajes se leen del proveedor de SMS del sistema y se muestran en el dispositivo. La función opcional de reenvío envía mensajes SMS a los números que el propio usuario configura, a través de la red del operador; sOCratic no recibe, no almacena y no comparte ninguna copia. Los números de reenvío se guardan únicamente en el almacenamiento local del teléfono.
```

---

# 5. Otros campos de la ficha

| Campo | Valor |
|---|---|
| Categoría | **Comunicación** *(no "Herramientas": una app de SMS predeterminada va en Comunicación)* |
| Etiquetas | sms, mensajes, app de sms, messaging, communication |
| Clasificación de contenido | Para todos |
| Correo de contacto | jsoladelarosa@gmail.com |
| Política de privacidad | https://sites.google.com/view/socraticweb/pàgina-principal |
| Precio | Gratis |
| Anuncios | No |
| Compras en la aplicación | No |

### Capturas de pantalla

**Obligatorio**: al menos dos capturas de la pantalla **Mensajes** y una de **Redacción**. Es la
función que justifica los permisos, y la política exige que esté promocionada en la ficha. Las
capturas actuales en `GooglePlayConsole/SMSForwarder/capturas/` (principal, menú, diagnóstico)
**no bastan**.

### Notas de la versión *(es-ES)*

```
• Nueva pantalla Mensajes: consulta tus SMS recibidos y enviados, márcalos como leídos, respóndelos tocándolos y bórralos deslizando.
• Nueva pantalla de redacción: escribe y envía SMS eligiendo el destinatario desde tus contactos, con contador de caracteres y de partes del mensaje.
• Ahora puedes responder a una llamada entrante con un mensaje.
• Corregida la detección de "app de SMS predeterminada" en dispositivos Xiaomi/HyperOS, que provocaba un aviso permanente y podía duplicar el reenvío.
```

---

# 6. Antes de darle a "Enviar a revisión"

- [ ] Bundle `2026.08.02.0` (versionCode `202608020`) subido y firmado con
      `Shared\socratic.keystore`, alias `smsforwarder`
- [ ] Declaración SMS/Call Log rehecha: **solo** `Default SMS handler`, con los tres permisos
- [ ] Enlace del vídeo (YouTube no listado) pegado en el formulario
- [ ] Capturas de Mensajes y Redacción subidas
- [ ] Ficha en es-ES actualizada con los textos de la sección 2
- [ ] Data safety respondido como "no recopila"
- [ ] Política de privacidad accesible y coherente con lo anterior
- [ ] Reenviar desde *Publishing overview*. **No apelar**: el rechazo era correcto sobre el APK
      anterior, lo que corresponde es enviar la versión corregida
