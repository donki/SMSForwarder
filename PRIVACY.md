# Política de Privacidad — Mensajes SMS Forwarder

**Aplicación:** Mensajes SMS Forwarder (`com.socratic.smsforwarder`)
**Responsable:** sOCratic
**Contacto:** jsoladelarosa@gmail.com
**Última actualización:** 7 de agosto de 2026

## Resumen

Mensajes SMS Forwarder es una aplicación de mensajes SMS que puedes establecer como tu aplicación
de SMS predeterminada en Android. **Todo el tratamiento de tus mensajes ocurre en tu dispositivo.**
sOCratic no recibe, no almacena y no comparte tus mensajes ni ningún otro dato personal. No hay
cuentas de usuario, ni servidores propios, ni analítica, ni publicidad, ni rastreadores.

La aplicación **no tiene permiso de acceso a internet** (`android.permission.INTERNET` no está
declarado en la aplicación), por lo que técnicamente no puede enviar información a ningún servidor.

## Qué datos trata la aplicación y para qué

| Dato | Para qué se usa | Dónde queda |
|---|---|---|
| Mensajes SMS recibidos | Mostrarlos en la pantalla Mensajes, notificarte de los nuevos y, si lo activas, reenviarlos | En el proveedor de SMS del sistema Android, en tu teléfono |
| Mensajes SMS enviados | Mostrarlos en la pestaña Enviados | En el proveedor de SMS del sistema Android, en tu teléfono |
| Números de destino del reenvío | Saber a qué números reenviar los mensajes que recibes | En el almacenamiento local de la aplicación, en tu teléfono |
| Número de contacto que eliges al redactar | Rellenar el destinatario del mensaje | No se guarda |

La aplicación no accede a tu agenda de contactos. Para elegir un destinatario se abre el **selector
de contactos del sistema**, que solo devuelve a la aplicación el número que tú has seleccionado; por
eso la aplicación no solicita el permiso `READ_CONTACTS`.

## Permisos que solicita y por qué

- **Leer SMS (`READ_SMS`)** — mostrar en la pantalla Mensajes el buzón de recibidos y enviados.
- **Recibir SMS (`RECEIVE_SMS`)** — recibir los mensajes entrantes cuando la aplicación es tu
  aplicación de SMS predeterminada.
- **Enviar SMS (`SEND_SMS`)** — enviar los mensajes que escribes, responder con un mensaje a una
  llamada entrante y realizar el reenvío que tú configures.
- **Notificaciones (`POST_NOTIFICATIONS`)** — avisarte cuando llega un mensaje nuevo.
- **Inicio tras reiniciar (`RECEIVE_BOOT_COMPLETED`)** y **servicio en primer plano
  (`FOREGROUND_SERVICE`)** — que el reenvío que has configurado siga funcionando después de
  reiniciar el teléfono.

Estos permisos se usan **únicamente** para las funciones descritas aquí y promocionadas en la ficha
de Google Play. No se usan para ninguna otra finalidad.

## Función de reenvío

El reenvío automático está **desactivado hasta que tú añades números de destino**. Cuando lo
activas, la aplicación reenvía por SMS los mensajes que recibes a los números que tú has indicado,
usando **la red de tu operador de telefonía**, igual que cualquier SMS que envíes a mano. Ese envío
puede tener el coste que tu operador aplique a un SMS.

En ese proceso **sOCratic no recibe ninguna copia** de tus mensajes: no hay ningún servidor
intermedio, el mensaje va de tu teléfono al número que tú has configurado.

Puedes ver, modificar o vaciar la lista de números de destino en cualquier momento desde la
configuración de la aplicación.

## Compartición y venta de datos

sOCratic **no vende, no cede y no comparte** con terceros datos personales ni datos sensibles
obtenidos a través de los permisos de SMS, ni para publicidad ni para ninguna otra finalidad.

## Conservación y borrado de datos

- Los mensajes se guardan en el proveedor de SMS del sistema Android. Puedes borrarlos desde la
  propia aplicación (deslizando sobre un mensaje) o desde cualquier otra aplicación de mensajes.
- Los números de reenvío se guardan solo en tu dispositivo y puedes eliminarlos desde la
  configuración.
- **Desinstalar la aplicación elimina los datos que la aplicación guarda** (números de reenvío y
  preferencias). Los mensajes SMS pertenecen al sistema Android y permanecen en el teléfono.

Como no se envía ningún dato fuera del dispositivo, no hay datos en poder de sOCratic que puedas
solicitar que se eliminen. Si tienes cualquier duda, escribe a jsoladelarosa@gmail.com.

## Seguridad

Los datos no salen del dispositivo, por lo que quedan protegidos por los mecanismos de seguridad y
cifrado del propio sistema Android y por el bloqueo de pantalla que tengas configurado.

## Menores

La aplicación no está dirigida a menores de 13 años y no recopila datos de ellos de forma
consciente.

## Uso responsable

Usa esta aplicación únicamente con líneas telefónicas y mensajes sobre los que tengas autorización.

## Cambios en esta política

Cualquier cambio se publicará en esta misma página, actualizando la fecha de «Última actualización».

## Contacto

Para cualquier cuestión relacionada con la privacidad: **jsoladelarosa@gmail.com**
