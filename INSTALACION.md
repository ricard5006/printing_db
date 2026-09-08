# Instalacion de Indigo AP - Etiquetas

## Requisitos previos

1. **Windows 7 SP1 o superior** (64 o 32 bits).
2. **.NET Framework 4.5 o superior.** El instalador verifica este requisito
   automaticamente y muestra un mensaje si falta. En Windows 10/11 ya viene
   incluido.
3. **SQL Server Express LocalDB** (instancia `(LocalDB)\MSSQLLocalDB`),
   version 2014 o superior. Si no esta instalado, la aplicacion muestra un
   aviso al abrirse y no arranca.

### Como instalar LocalDB

1. Descargue el instalador **SqlLocalDB.msi** desde la pagina oficial de
   SQL Server Express:
   `https://download.microsoft.com/download/3/8/d/38de7036-2433-4207-8eae-06e247e17b25/SqlLocalDB.msi`
2. Ejecute el archivo descargado y acepte la instalacion con los valores
   predeterminados. No requiere configuracion adicional.
3. Puede verificar que la instancia este disponible (desde una consola `cmd` o
   PowerShell):

   ```
   sqllocaldb info
   ```

   Debe aparecer la instancia `MSSQLLocalDB`.

## Instalacion de la aplicacion

1. Ejecute el instalador:

   ```
   Setup_indigo.msi
   ```

   (doble clic sobre el archivo, y siga el asistente).

2. La aplicacion se instala en:

   ```
   C:\Program Files (x86)\IndigoApps\Indigo AP - Etiquetas\
   ```

   y se crean accesos directos en el menu Programas y en el Escritorio.

3. Al ejecutar la aplicacion por primera vez, esta crea su carpeta de datos
   en el perfil del usuario:

   ```
   %LocalAppData%\IndigoApps\
   ```

   La carpeta contiene la base de datos `indigo_app.mdf`, el archivo de
   activacion `Activacion.txt` y el archivo de registro `log.txt`. La base
   de datos se crea automaticamente con la version de LocalDB instalada en
   el equipo; no hace falta hacer nada.

   Nota: programe la activacion utilizada el mismo equipo. La base de datos
   queda vacia (sin datos de impresion previos).

### Problemas conocidos

**Error: "The database ...\INDIGO_APP.MDF cannot be opened because it is
version XXXX. This server supports version YYYY and earlier."**

Indica que la base existente en `%LocalAppData%\IndigoApps` fue creada con
una version de LocalDB mas nueva que la instalada en el equipo. Solucion:
borre los archivos `indigo_app.mdf` e `indigo_app_log.ldf` de esa carpeta y
vuelva a abrir la aplicacion; esta la regenerara con la version compatible.

## Actualizar a una version nueva

El instalador esta configurado con `RemovePreviousVersions` activado:
instale la nueva version sobre la instalacion anterior y esta se reemplaza
automaticamente. La carpeta de datos `%LocalAppData%\IndigoApps` no se
toca, por lo que se conservan la activacion y los datos almacenados.

## Desinstalacion

1. Desde **Panel de control** -> **Programas** -> **Programas y
   caracteristicas**, seleccione "Indigo AP - Etiquetas" y pulse
   Desinstalar.

   Alternativa por linea de comandos:

   ```
   msiexec /x {A1B2C3D4-5E6F-4A7B-8C9D-0E1F2A3B4C5D}
   ```

2. La desinstalacion elimina los archivos del programa y los accesos
   directos. La carpeta de datos `%LocalAppData%\IndigoApps` se conserva
   (si desea eliminarla, borrela manualmente).

