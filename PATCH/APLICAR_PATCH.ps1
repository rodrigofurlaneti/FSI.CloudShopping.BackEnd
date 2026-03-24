# ================================================================
# APLICAR_PATCH.ps1  —  FSI.CloudShopping PATCH DEFINITIVO v3
#
# Problema raiz resolvido:
#   SagaOrchestratorBase.cs foi movido de volta para Domain/Sagas,
#   mas SEM dependencia de ILogger (Domain deve ser puro).
#   O logging foi extraido para hooks virtuais que o Orchestrator
#   da camada Application sobrescreve com ILogger<T>.
#
# O que este script faz:
#   1. Copia SagaOrchestratorBase.cs (limpo) para Domain/Sagas/
#   2. Deleta SagaOrchestratorBase.cs da Application/Sagas/ (se existir)
#   3. Copia OrderCheckoutSagaOrchestrator.cs (atualizado) para Application/Sagas/
#   4. Copia as interfaces SAGA para Domain/Sagas/
#   5. Limpa obj/ para recompilacao limpa
#
# Como executar (PowerShell na pasta onde este .ps1 esta):
#   Set-ExecutionPolicy Bypass -Scope Process
#   .\APLICAR_PATCH.ps1
# ================================================================

param(
    [string]$RepoRoot = "C:\Users\AMD\Source\Repos\FSI.CloudShopping.BackEnd"
)

$scriptDir    = $PSScriptRoot
$domainSagas  = "$RepoRoot\src\FSI.CloudShopping.Domain\Sagas"
$appSagas     = "$RepoRoot\src\FSI.CloudShopping.Application\Sagas"

Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  FSI CloudShopping — Patch Definitivo v3  " -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# ── 1. Garantir pastas ───────────────────────────────────────────
if (-not (Test-Path $domainSagas)) { New-Item -ItemType Directory -Path $domainSagas | Out-Null }
if (-not (Test-Path $appSagas))    { New-Item -ItemType Directory -Path $appSagas    | Out-Null }

# ── 2. Copiar SagaOrchestratorBase.cs para Domain (sem ILogger) ──
Write-Host "[1/4] Copiando SagaOrchestratorBase.cs para Domain/Sagas/..." -ForegroundColor Yellow
Copy-Item "$scriptDir\Domain\Sagas\SagaOrchestratorBase.cs" "$domainSagas\SagaOrchestratorBase.cs" -Force
Write-Host "      OK: $domainSagas\SagaOrchestratorBase.cs" -ForegroundColor Green

# ── 3. DELETAR SagaOrchestratorBase.cs da Application (se existir) ─
Write-Host ""
Write-Host "[2/4] Removendo SagaOrchestratorBase.cs da Application/Sagas/ (se existir)..." -ForegroundColor Yellow
$appBase = "$appSagas\SagaOrchestratorBase.cs"
if (Test-Path $appBase) {
    Remove-Item $appBase -Force
    Write-Host "      DELETADO: $appBase" -ForegroundColor Green
} else {
    Write-Host "      Nao encontrado (ok): $appBase" -ForegroundColor Gray
}

# ── 4. Copiar OrderCheckoutSagaOrchestrator.cs para Application ──
Write-Host ""
Write-Host "[3/4] Copiando OrderCheckoutSagaOrchestrator.cs para Application/Sagas/..." -ForegroundColor Yellow
Copy-Item "$scriptDir\Application\Sagas\OrderCheckoutSagaOrchestrator.cs" "$appSagas\OrderCheckoutSagaOrchestrator.cs" -Force
Write-Host "      OK: $appSagas\OrderCheckoutSagaOrchestrator.cs" -ForegroundColor Green

# ── 5. Copiar interfaces SAGA para Domain ────────────────────────
Write-Host ""
Write-Host "[4/4] Copiando interfaces SAGA para Domain/Sagas/..." -ForegroundColor Yellow
foreach ($f in @("ISagaLogger.cs","ISagaOrchestrator.cs","ISagaState.cs","ISagaStep.cs")) {
    $src = "$scriptDir\Domain\Sagas\$f"
    if (Test-Path $src) {
        Copy-Item $src "$domainSagas\$f" -Force
        Write-Host "      OK: $f" -ForegroundColor Green
    }
}

# ── 6. Limpar obj/ ───────────────────────────────────────────────
Write-Host ""
Write-Host "[Bonus] Limpando pastas obj/ para recompilacao limpa..." -ForegroundColor Yellow
foreach ($proj in @("FSI.CloudShopping.Domain","FSI.CloudShopping.Application",
                    "FSI.CloudShopping.Infrastructure","FSI.CloudShopping.WebAPI")) {
    $obj = "$RepoRoot\src\$proj\obj"
    if (Test-Path $obj) { Remove-Item $obj -Recurse -Force; Write-Host "      Limpo: $proj\obj" -ForegroundColor Green }
}

Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  PATCH APLICADO!                          " -ForegroundColor Green
Write-Host "  Visual Studio: Build > Rebuild Solution  " -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
