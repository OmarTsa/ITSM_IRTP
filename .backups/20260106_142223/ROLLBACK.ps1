# ════════════════════════════════════════════════════════════════
# SCRIPT DE ROLLBACK - Restaurar versión anterior
# Backup: 20260106_142223
# ════════════════════════════════════════════════════════════════

Write-Host "🔄 Restaurando archivos desde backup..." -ForegroundColor Yellow

Copy-Item -Path "D:\Proyectos\Tesis\ITSM_IRTP\.backups\20260106_142223\ITSM.WEB.Client_Program.cs" -Destination "D:\Proyectos\Tesis\ITSM_IRTP\ITSM.WEB.Client\Program.cs" -Force
Copy-Item -Path "D:\Proyectos\Tesis\ITSM_IRTP\.backups\20260106_142223\ITSM.WEB.Client_Auth_ManejadorAutorizacionPersonalizado.cs" -Destination "D:\Proyectos\Tesis\ITSM_IRTP\ITSM.WEB.Client\Auth\ManejadorAutorizacionPersonalizado.cs" -Force

Write-Host "✅ Rollback completado" -ForegroundColor Green
Write-Host "⚠️  Recuerda recompilar la solución" -ForegroundColor Yellow
