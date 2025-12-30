# Script para organizar la estructura del proyecto ITSM.WEB.Client
# Ruta: D:\Proyectos\Tesis\ITSM_IRTP\ITSM.WEB.Client\OrganizarProyecto.ps1

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Organizando Proyecto ITSM.WEB.Client" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$rootPath = Get-Location

# 1. Crear estructura de carpetas
Write-Host "1. Creando estructura de carpetas..." -ForegroundColor Yellow

$folders = @(
    "Pages",
    "Pages\Admin",
    "Pages\HelpDesk",
    "Pages\Inventario",
    "Layout",
    "Servicios",
    "Auth"
)

foreach ($folder in $folders) {
    $fullPath = Join-Path $rootPath $folder
    if (-not (Test-Path $fullPath)) {
        New-Item -ItemType Directory -Path $fullPath -Force | Out-Null
        Write-Host "  ✓ Creada: $folder" -ForegroundColor Green
    } else {
        Write-Host "  - Ya existe: $folder" -ForegroundColor Gray
    }
}

Write-Host ""

# 2. Mover archivos de Layout
Write-Host "2. Organizando archivos de Layout..." -ForegroundColor Yellow

$layoutFiles = @{
    "MainLayout.razor" = "Layout\MainLayout.razor"
    "NavMenu.razor" = "Layout\NavMenu.razor"
    "LoginLayout.razor" = "Layout\LoginLayout.razor"
}

foreach ($file in $layoutFiles.Keys) {
    $source = Join-Path $rootPath $file
    $dest = Join-Path $rootPath $layoutFiles[$file]
    
    if (Test-Path $source) {
        Move-Item -Path $source -Destination $dest -Force
        Write-Host "  ✓ Movido: $file → $($layoutFiles[$file])" -ForegroundColor Green
    }
}

Write-Host ""

# 3. Mover archivos de Admin
Write-Host "3. Organizando páginas de Admin..." -ForegroundColor Yellow

$adminFiles = @(
    "Usuarios.razor",
    "FormUsuario.razor",
    "GestionTipos.razor"
)

foreach ($file in $adminFiles) {
    $source = Join-Path $rootPath $file
    $dest = Join-Path $rootPath "Pages\Admin\$file"
    
    if (Test-Path $source) {
        Move-Item -Path $source -Destination $dest -Force
        Write-Host "  ✓ Movido: $file → Pages\Admin\$file" -ForegroundColor Green
    } else {
        # Buscar en subcarpetas
        $found = Get-ChildItem -Recurse -Filter $file -ErrorAction SilentlyContinue | Select-Object -First 1
        if ($found) {
            Move-Item -Path $found.FullName -Destination $dest -Force
            Write-Host "  ✓ Movido: $($found.FullName) → Pages\Admin\$file" -ForegroundColor Green
        } else {
            Write-Host "  ⚠ No encontrado: $file" -ForegroundColor Red
        }
    }
}

Write-Host ""

# 4. Mover archivos de HelpDesk
Write-Host "4. Organizando páginas de HelpDesk..." -ForegroundColor Yellow

$helpdeskFiles = @(
    "MisTickets.razor",
    "NuevoTicket.razor",
    "DetalleTicket.razor",
    "GestionTickets.razor"
)

foreach ($file in $helpdeskFiles) {
    $found = Get-ChildItem -Recurse -Filter $file -ErrorAction SilentlyContinue | Select-Object -First 1
    if ($found) {
        $dest = Join-Path $rootPath "Pages\HelpDesk\$file"
        Move-Item -Path $found.FullName -Destination $dest -Force
        Write-Host "  ✓ Movido: $file → Pages\HelpDesk\$file" -ForegroundColor Green
    } else {
        Write-Host "  ⚠ No encontrado: $file" -ForegroundColor Red
    }
}

Write-Host ""

# 5. Mover archivos de Inventario
Write-Host "5. Organizando páginas de Inventario..." -ForegroundColor Yellow

$inventarioFiles = @(
    "Inventario.razor",
    "NuevoActivo.razor"
)

foreach ($file in $inventarioFiles) {
    $found = Get-ChildItem -Recurse -Filter $file -ErrorAction SilentlyContinue | Select-Object -First 1
    if ($found) {
        $dest = Join-Path $rootPath "Pages\Inventario\$file"
        Move-Item -Path $found.FullName -Destination $dest -Force
        Write-Host "  ✓ Movido: $file → Pages\Inventario\$file" -ForegroundColor Green
    } else {
        Write-Host "  ⚠ No encontrado: $file" -ForegroundColor Red
    }
}

Write-Host ""

# 6. Mover archivos principales a Pages
Write-Host "6. Organizando páginas principales..." -ForegroundColor Yellow

$mainPages = @(
    "Dashboard.razor",
    "Login.razor",
    "Counter.razor",
    "Reportes.razor"
)

foreach ($file in $mainPages) {
    $found = Get-ChildItem -Recurse -Filter $file -ErrorAction SilentlyContinue | Select-Object -First 1
    if ($found) {
        $dest = Join-Path $rootPath "Pages\$file"
        Move-Item -Path $found.FullName -Destination $dest -Force
        Write-Host "  ✓ Movido: $file → Pages\$file" -ForegroundColor Green
    } else {
        Write-Host "  ⚠ No encontrado: $file" -ForegroundColor Red
    }
}

Write-Host ""

# 7. Mover servicios
Write-Host "7. Organizando servicios..." -ForegroundColor Yellow

$servicioFiles = @(
    "IServicioSesion.cs",
    "ServicioSesion.cs",
    "TicketServicio.cs",
    "InventarioServicio.cs",
    "UsuarioServicio.cs"
)

foreach ($file in $servicioFiles) {
    $found = Get-ChildItem -Recurse -Filter $file -ErrorAction SilentlyContinue | Select-Object -First 1
    if ($found) {
        $dest = Join-Path $rootPath "Servicios\$file"
        Move-Item -Path $found.FullName -Destination $dest -Force
        Write-Host "  ✓ Movido: $file → Servicios\$file" -ForegroundColor Green
    } else {
        Write-Host "  ⚠ No encontrado: $file" -ForegroundColor Red
    }
}

Write-Host ""

# 8. Verificar Auth
Write-Host "8. Verificando carpeta Auth..." -ForegroundColor Yellow

$authFile = "ProveedorAutenticacion.cs"
$found = Get-ChildItem -Recurse -Filter $authFile -ErrorAction SilentlyContinue | Select-Object -First 1
if ($found -and $found.DirectoryName -notlike "*\Auth") {
    $dest = Join-Path $rootPath "Auth\$authFile"
    Move-Item -Path $found.FullName -Destination $dest -Force
    Write-Host "  ✓ Movido: $authFile → Auth\$authFile" -ForegroundColor Green
} else {
    Write-Host "  - Ya está en Auth o no encontrado" -ForegroundColor Gray
}

Write-Host ""

# 9. Limpiar carpetas vacías
Write-Host "9. Limpiando carpetas vacías..." -ForegroundColor Yellow

Get-ChildItem -Recurse -Directory | 
    Where-Object { (Get-ChildItem $_.FullName -Recurse -Force | Measure-Object).Count -eq 0 } |
    Remove-Item -Recurse -Force

Write-Host "  ✓ Carpetas vacías eliminadas" -ForegroundColor Green

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  ✓ Organización completada" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# 10. Mostrar estructura final
Write-Host "Estructura final:" -ForegroundColor Yellow
tree /F /A
