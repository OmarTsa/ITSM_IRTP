:root {
    /* Colores Corporativos OTIC */
    --otic-red-primary: #B91C2E;
    --otic - red - dark: #8B1523;
    --otic - red - light: #D4364A;
    --otic - white: #FFFFFF;
    --otic - gray - light: #F5F5F5;
    --otic - gray: #E0E0E0;
    --otic - text - dark: #2C3E50;
}

/* ============================================
   UTILIDADES GLOBALES
   ============================================ */
.otic - brand {
background: linear - gradient(135deg, var(--otic - red - primary) 0 %, var(--otic - red - dark) 100 %);
color: var(--otic - white);
}

.logo - white {
filter: brightness(0) invert(1);
}

.logo - normal {
filter: none;
}

/* ============================================
   LOGIN PAGE
   ============================================ */
.login - container {
background: linear - gradient(135deg, #B91C2E 0%, #8B1523 100%);
    min - height: 100vh;
display: flex;
    align - items: center;
    justify - content: center;
position: relative;
overflow: hidden;
}

.login - container::before {
content: '';
position: absolute;
top: -50 %;
right: -50 %;
width: 200 %;
height: 200 %;
background: radial - gradient(circle, rgba(255, 255, 255, 0.1) 0 %, transparent 70 %);
animation: pulse 15s ease-in-out infinite;
}

@keyframes pulse
{
    0%, 100% { transform: scale(1); opacity: 0.5; }
    50% { transform: scale(1.1); opacity: 0.3; }
}

.login - card {
background: white;
    border - radius: 20px;
    box - shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
padding: 40px;
    max - width: 450px;
width: 100 %;
position: relative;
    z - index: 1;
}

.otic - logo - container {
    text - align: center;
padding: 20px 0;
    margin - bottom: 20px;
}

.otic - logo {
    max - width: 200px;
height: auto;
    margin - bottom: 10px;
}

/* ============================================
   DASHBOARD KPI CARDS
   ============================================ */
.kpi - card {
    border - radius: 16px!important;
transition: transform 0.3s ease, box-shadow 0.3s ease;
overflow: hidden;
position: relative;
    border - left: 5px solid;
}

.kpi - card:hover {
    transform: translateY(-5px);
box - shadow: 0 12px 24px rgba(0, 0, 0, 0.15) !important;
}

.kpi - card - otic {
    border - left - color: #B91C2E !important; }
.kpi - card - warning {
        border - left - color: #FF9800 !important; }
.kpi - card - info {
            border - left - color: #2196F3 !important; }
.kpi - card - success {
                border - left - color: #4CAF50 !important; }

.kpi - icon {
                    font - size: 48px!important;
                opacity: 0.3;
                }

/* ============================================
   APPBAR PERSONALIZADO
   ============================================ */
.otic - appbar {
                background: linear - gradient(90deg, #B91C2E 0%, #8B1523 100%) !important;
    box - shadow: 0 4px 12px rgba(0, 0, 0, 0.15)!important;
                }

/* ============================================
   SIDEBAR BRANDING
   ============================================ */
.sidebar - brand {
                background: var(--otic - red - primary);
                padding: 20px;
                    text - align: center;
                    border - radius: 0 0 16px 16px;
                    margin - bottom: 20px;
                }

.sidebar - logo {
                    max - width: 120px;
                height: auto;
                filter: brightness(0) invert(1);
                }

/* ============================================
   BOTONES PERSONALIZADOS
   ============================================ */
.otic - btn - primary {
                    background - color: var(--otic - red - primary)!important;
                color: var(--otic - white)!important;
                }

.otic - btn - primary:hover {
                    background - color: var(--otic - red - dark)!important;
                }

/* ============================================
   TABLAS MEJORADAS
   ============================================ */
.otic - table {
                    border - radius: 12px;
                overflow: hidden;
                }

.otic - table.mud - table - head {
                background: var(--otic - gray - light);
                }

.otic - table.mud - table - row:hover {
                background: rgba(185, 28, 46, 0.05);
                }

/* ============================================
   CHIPS DE ESTADO
   ============================================ */
.chip - abierto {
                    background - color: #FF5252 !important;
    color: white!important;
                }

.chip - proceso {
                    background - color: #2196F3 !important;
    color: white!important;
                }

.chip - resuelto {
                    background - color: #4CAF50 !important;
    color: white!important;
                }

.chip - cerrado {
                    background - color: #757575 !important;
    color: white!important;
                }

/* ============================================
   MEJORAS VISUALES PROFESIONALES
   ============================================ */
.page - header {
                    margin - bottom: 24px;
                    padding - bottom: 16px;
                    border - bottom: 3px solid var(--otic - red - primary);
                }

.action - toolbar {
                background: var(--otic - gray - light);
                padding: 16px;
                    border - radius: 12px;
                    margin - bottom: 20px;
                }

                /* ============================================
                   ANIMACIONES
                   ============================================ */
                @keyframes fadeIn {
                    from { opacity: 0; transform: translateY(20px); }
                    to { opacity: 1; transform: translateY(0); }
                }

.fade -in {
                animation: fadeIn 0.5s ease-out;
                }

                @keyframes slideIn {
                    from { transform: translateX(-20px); opacity: 0; }
                    to { transform: translateX(0); opacity: 1; }
                }

.slide -in {
                animation: slideIn 0.4s ease-out;
                }

/* ============================================
   CARDS PERSONALIZADOS
   ============================================ */
.otic - card {
                    border - radius: 16px!important;
                overflow: hidden;
                transition: all 0.3s ease;
                }

.otic - card:hover {
                    box - shadow: 0 8px 24px rgba(0, 0, 0, 0.12) !important;
                }

.otic - card - header {
                background: var(--otic - red - primary);
                color: var(--otic - white);
                padding: 16px 24px;
                    font - weight: 600;
                }

/* ============================================
   ACCESOS RÁPIDOS
   ============================================ */
.quick - access - btn {
                height: 60px;
                    border - radius: 10px!important;
                    font - weight: 600;
                transition: all 0.3s ease;
                }

.quick - access - btn:hover {
                transform: translateY(-3px);
                    box - shadow: 0 6px 20px rgba(0, 0, 0, 0.15);
                }

                /* ============================================
                   RESPONSIVE
                   ============================================ */
                @media(max - width: 768px) {
    .login - card {
                    margin: 20px;
                    padding: 30px 20px;
                    }
    
    .otic - logo {
                        max - width: 150px;
                    }

    .sidebar - brand {
                    padding: 15px;
                    }

    .kpi - card {
                        margin - bottom: 16px;
                    }
                }

                @media(max - width: 480px) {
    .login - card {
                    padding: 20px 15px;
                    }

    .otic - logo {
                        max - width: 120px;
                    }

    .quick - access - btn {
                    height: 50px;
                        font - size: 0.875rem;
                    }
                }

/* ============================================
   MEJORAS MUDBLAZOR
   ============================================ */
.mud - appbar {
                    z - index: 1200!important;
                }

.mud - drawer {
                    z - index: 1100!important;
                }

.mud - overlay - drawer {
                    z - index: 1099!important;
                }

/* Menu items activos */
.mud - nav - link - active {
                    background - color: rgba(185, 28, 46, 0.1)!important;
                    border - left: 4px solid var(--otic - red - primary)!important;
                }

/* Scrollbar personalizado */
::- webkit - scrollbar {
                width: 8px;
                height: 8px;
                }

::- webkit - scrollbar - track {
                background: #f1f1f1;
}

::- webkit - scrollbar - thumb {
                background: #888;
    border - radius: 4px;
                }

::- webkit - scrollbar - thumb:hover {
                background: #555;
}
