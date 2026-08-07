# Declaración SMS/Call Log en Google Play — SMS Forwarder

Material de apoyo para el reenvío a revisión tras el rechazo del **2026-08-02**
(*Ineligible use case: Initiate a text message, Initiate a phone call (Fast Dial)*, routing ID `ZLFS`).

---

## 1. Por qué nos rechazaron

El APK declaraba el rol de **app de SMS por defecto** con sus cuatro componentes obligatorios,
pero tres de ellos no hacían nada:

| Componente | Antes | Cómo lo lee el revisor |
|---|---|---|
| `ComposeSmsActivity` (`SENDTO`/`SEND`/`VIEW` sobre `sms:`) | Redirigía a la pantalla principal | *Initiate a text message* |
| `HeadlessSmsSendService` (`RESPOND_VIA_MESSAGE`) | No-op | Familia *Fast Dial* |
| `READ_SMS` | Declarado, sin ninguna pantalla que leyera el buzón | Permiso sin función |

La política citada en el correo lo dice literal: no se pueden pedir permisos sensibles para
funciones *"undisclosed, unimplemented, or disallowed"*. Ambos casos imputados tienen alternativa
sin permiso (SMS Intent y Dial Intent), así que Google no concede el permiso para ellos.

## 2. Qué se ha corregido (versión 2026.08.02.0)

- **Buzón real** (`Pages/MessagesPage.xaml`): lista recibidos y enviados del proveedor del
  sistema, marca como leído, borra deslizando y responde al tocar. Justifica `READ_SMS`.
- **Redacción real** (`Pages/ComposePage.xaml`): destinatario + selector de contactos del sistema,
  contador de caracteres/partes y envío guardado en Enviados.
- **`ComposeSmsActivity`**: extrae destinatario y texto del intent (`sms_body`, `EXTRA_TEXT`,
  parámetro `body`) y abre la redacción con los campos rellenos.
- **`HeadlessSmsSendService`**: envía de verdad la respuesta rápida a una llamada entrante.
- **"Mensajes" es la primera entrada del menú** y ofrece convertir la app en la predeterminada.

Resultado: el caso de uso declarable pasa a ser **Default SMS handler**, que sí es un uso
permitido, y los cuatro componentes están implementados.

## 3. Declaración en Play Console

**Ruta:** Play Console → Política → Contenido de la aplicación → Permisos de SMS y registro de llamadas.

- **Caso de uso principal:** `Default SMS handler` (gestor de SMS predeterminado).
- **NO marcar:** *Initiate a text message*, *Initiate a phone call (Fast Dial)*, ni ningún caso de
  la lista de usos no permitidos.
- **Permisos declarados:** `RECEIVE_SMS`, `SEND_SMS`, `READ_SMS`.

**Justificación (texto para el formulario):**

> SMS Forwarder es una aplicación de mensajes que funciona como gestor de SMS predeterminado del
> teléfono. Implementa los cuatro componentes que Android exige para ese rol: recepción
> (`SMS_DELIVER`), recepción WAP push (`WAP_PUSH_DELIVER`), redacción desde otras aplicaciones
> (`SENDTO`) y respuesta rápida a llamadas (`RESPOND_VIA_MESSAGE`).
>
> `READ_SMS` se usa para mostrar el buzón de mensajes recibidos y enviados en la pantalla
> "Mensajes". `RECEIVE_SMS` para recibir los mensajes entrantes. `SEND_SMS` para enviar mensajes
> desde la pantalla de redacción, para la respuesta rápida a una llamada entrante y para el
> reenvío automático que el usuario configura.
>
> Los datos no salen del dispositivo: los mensajes se leen del proveedor del sistema y se muestran
> localmente, y los números de reenvío se guardan solo en el teléfono. No hay servidores, ni
> analítica, ni venta ni cesión de datos.

## 4. Ficha de Play (obligatorio: la función debe estar promocionada)

**Descripción corta (máx. 80 caracteres):**

> App de mensajes SMS con reenvío automático a los números que elijas.

**Descripción larga (apertura):**

> SMS Forwarder es tu aplicación de mensajes: lee, escribe, responde y borra tus SMS desde una
> interfaz sencilla, y además reenvía automáticamente los mensajes que recibes a los números que
> tú configures.
>
> **Mensajes**
> • Buzón de mensajes recibidos y enviados
> • Escribe y envía SMS, eligiendo el número desde tus contactos
> • Responde tocando un mensaje
> • Marca como leído y borra deslizando
> • Responde con un mensaje a una llamada entrante
>
> **Reenvío automático**
> • Reenvía los SMS que recibes a uno o varios números
> • Protección contra bucles de reenvío
> • Funciona en segundo plano y tras reiniciar el teléfono
>
> **Privacidad**
> • Todo ocurre en tu teléfono: sin servidores, sin cuentas, sin seguimiento
> • No se venden ni se comparten datos personales
> • Software libre con licencia MIT

**Capturas:** al menos una del buzón, una de la redacción y una de la configuración de reenvío.

## 5. Guión del vídeo de demostración

Súbelo a YouTube **como "no listado"** y pega el enlace en el formulario. Sin cortes, 60-90 s:

1. Ajustes de Android → aplicación de SMS predeterminada → elegir **SMS Forwarder**.
2. Abrir la app: se ve la pantalla **Mensajes** con el buzón cargado.
3. Tocar un mensaje → se abre la respuesta con el remitente relleno → escribir y **enviar**.
4. Volver, pestaña **Enviados**: aparece el mensaje recién enviado.
5. Deslizar un mensaje → **Eliminar** → confirmar.
6. Menú → **Configuración** → añadir un número de reenvío.
7. Enviar un SMS al teléfono desde otro móvil → llega la notificación → se reenvía solo al número
   configurado (mostrar el segundo teléfono recibiéndolo).

## 6. Cómo generar y subir el bundle

El keystore es el **compartido** de todas las apps sOCratic, no una copia local:
`..\Shared\socratic.keystore`, alias `smsforwarder`. La contraseña no está en el repo
(ver `Hiker\SECRETS.md`); se pasa por variable de entorno o por parámetro.

> ⚠️ **No confundir con `Hiker\Hiker\socratic.keystore`**. Es un fichero *distinto*, contiene
> solo el alias `hiker` (SHA1 `F9:04:79:…`) y su contraseña (`keystore.password.txt`, `socratic2026`)
> **no** abre el keystore compartido. Firmar con él haría que Play rechazara el bundle por
> certificado incorrecto.

```powershell
# 1. Contrasena del keystore compartido (una vez por sesion)
$env:ANDROID_KEYSTORE_PASSWORD = '<contrasena del keystore compartido>'

# 2. AAB firmado -> bin\Release\net9.0-android36.0\publish\com.socratic.smsforwarder-Signed.aab
.\build_and_sign.ps1 -SkipApk -NoPause

# 3. Validar contra Play SIN publicar (recomendado antes del commit real)
$env:GOOGLE_APPLICATION_CREDENTIALS = 'D:\sOCProjects\Mobile\Hiker\Hiker\hiker-433118-98861f2881fa.json'
pwsh .\publish_aab_to_play.ps1 -Track internal -Status draft -ValidateOnly

# 4. Subida real a internal testing
pwsh .\publish_aab_to_play.ps1 -Track internal -Status draft
```

`publish_aab_to_play.ps1` **sobrescribe la ficha** con `PlayStoreListing.es-ES.json`. Ese fichero
ya lleva el texto nuevo centrado en la app de mensajes; si alguna vez vuelve al texto viejo
centrado en el reenvío, se estaría subiendo justo lo que Google rechazó. Usa `-SkipStoreListing`
si solo quieres subir el binario.

## 7. Checklist antes de reenviar a revisión

Verificado ya en el repo (2026-08-02):

- [x] Compila en Release sin errores, `versionCode` 202608020 / `2026.08.02.0`
- [x] Los cuatro componentes obligatorios implementados y declarados: `SmsDeliverReceiver`
      (`SMS_DELIVER`), `MmsDeliverReceiver` (`WAP_PUSH_DELIVER`), `ComposeSmsActivity`
      (`SENDTO`/`VIEW`/`SEND` sobre `sms:`), `HeadlessSmsSendService` (`RESPOND_VIA_MESSAGE`)
- [x] Ficha (`PlayStoreListing.es-ES.json` y `ficha.md`) reescrita: la app se presenta como
      gestor de SMS predeterminado y el reenvío queda como función secundaria
- [x] Permisos de la ficha = permisos del manifest. Se corrigió `ficha.md`, que declaraba
      `READ_CONTACTS` e `INTERNET`: ninguno de los dos se pide (contactos usa `ACTION_PICK`
      y la app no hace peticiones de red)

Pendiente, requiere acción manual:

- [ ] Generar el AAB firmado con la contraseña del keystore compartido (sección 6)
- [ ] **Capturas de la pantalla Mensajes y de Redacción** — `GooglePlayConsole/SMSForwarder/capturas`
      solo tiene principal, menú y diagnóstico. La política exige que la función que justifica los
      permisos esté promocionada en la ficha; sin estas capturas es rechazo probable
- [ ] Vídeo de demostración (guión en la sección 5) subido a YouTube como *no listado*
- [ ] Declaración SMS/Call Log rehecha con `Default SMS handler` y sin casos no permitidos
- [ ] Política de privacidad accesible y coherente con lo declarado
- [ ] Sección Seguridad de los datos respondida como **"no recopila datos"**. En la definición de
      Google, *recopilar* es transmitir datos fuera del dispositivo al desarrollador: la app no
      declara `INTERNET` ni hace peticiones de red, y el reenvío va por la red del operador a
      números que elige el usuario, no a sOCratic. Texto de apoyo en
      [TEXTOS-PLAY-COPIAR-PEGAR.md](TEXTOS-PLAY-COPIAR-PEGAR.md), sección 4
- [ ] Reenviar desde *Publishing overview* (no apelar: el rechazo era correcto sobre el APK anterior)
