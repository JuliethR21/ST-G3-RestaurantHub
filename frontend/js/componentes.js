// Piezas compartidas por todas las páginas: menú, ventanas emergentes, avisos y formatos.
// Cada página crea su app con crearApp({...}) en lugar de Vue.createApp, así ya tiene todo esto registrado.

// ---------- Avisos (mensajes flotantes de éxito o error) ----------
const avisos = Vue.reactive([]);

function notificar(tipo, texto) {
    const id = Date.now() + Math.random();
    avisos.push({ id, tipo, texto });
    setTimeout(() => {
        const indice = avisos.findIndex(a => a.id === id);
        if (indice >= 0) avisos.splice(indice, 1);
    }, 5000);
}

// ---------- Estados del pedido ----------
const ESTADOS_PEDIDO = {
    PENDIENTE: { texto: 'Pendiente', clase: 'text-bg-warning', icono: 'bi-hourglass-split' },
    PREPARANDO: { texto: 'En preparación', clase: 'text-bg-info', icono: 'bi-fire' },
    ENTREGADO: { texto: 'Entregado', clase: 'text-bg-success', icono: 'bi-check2-circle' },
    CANCELADO: { texto: 'Cancelado', clase: 'text-bg-secondary', icono: 'bi-x-circle' },
};

// ---------- Formatos ----------
const formatoMoneda = new Intl.NumberFormat('es-CO', { style: 'currency', currency: 'COP', maximumFractionDigits: 0 });
const formatoNumero = new Intl.NumberFormat('es-CO', { maximumFractionDigits: 2 });
const UNIDADES = { UNIDAD: 'und', GRAMO: 'g', KILOGRAMO: 'kg', MILILITRO: 'ml', LITRO: 'L', TAJADA: 'tajadas' };

const formatos = {
    moneda: valor => formatoMoneda.format(valor ?? 0),
    fecha: valor => valor ? new Date(valor).toLocaleString('es-CO', { dateStyle: 'medium', timeStyle: 'short' }) : '',
    cantidad: (valor, unidad) => `${formatoNumero.format(valor ?? 0)} ${UNIDADES[unidad] ?? (unidad ?? '').toLowerCase()}`,
};

// ---------- Menú lateral + encabezado + avisos ----------
const OPCIONES_MENU = [
    { clave: 'inicio', texto: 'Inicio', icono: 'bi-grid', href: 'index.html' },
    { clave: 'nuevo-pedido', texto: 'Nuevo pedido', icono: 'bi-plus-circle', href: 'nuevo-pedido.html' },
    { clave: 'pedidos', texto: 'Pedidos', icono: 'bi-receipt', href: 'pedidos.html' },
    { clave: 'productos', texto: 'Productos', icono: 'bi-egg-fried', href: 'productos.html' },
    { clave: 'categorias', texto: 'Categorías', icono: 'bi-tags', href: 'categorias.html' },
    { clave: 'inventario', texto: 'Inventario', icono: 'bi-box-seam', href: 'inventario.html' },
];

const AppLayout = {
    props: { activa: String, titulo: String, subtitulo: String },
    data: () => ({ opciones: OPCIONES_MENU, avisos }),
    template: `
        <div class="d-lg-flex">
            <aside class="sidebar offcanvas-lg offcanvas-start" tabindex="-1" id="menu-lateral">
                <div class="offcanvas-header">
                    <div class="logo"><span class="logo-icono">R</span> RestaurantHub</div>
                    <button type="button" class="btn-close btn-close-white d-lg-none" data-bs-dismiss="offcanvas"
                            data-bs-target="#menu-lateral" aria-label="Cerrar menú"></button>
                </div>
                <div class="offcanvas-body flex-column px-3">
                    <!-- En pantallas grandes Bootstrap oculta el offcanvas-header, por eso el logo se repite aquí -->
                    <div class="logo d-none d-lg-flex px-2 pt-3 pb-2"><span class="logo-icono">R</span> RestaurantHub</div>
                    <p class="menu-titulo">Menú principal</p>
                    <nav class="d-flex flex-column gap-1">
                        <a v-for="opcion in opciones" :key="opcion.clave" :href="opcion.href"
                           class="menu-enlace" :class="{ activo: opcion.clave === activa }">
                            <i class="bi" :class="opcion.icono"></i> {{ opcion.texto }}
                        </a>
                    </nav>
                </div>
            </aside>

            <div class="contenido flex-grow-1">
                <header class="d-flex align-items-center gap-3 px-3 px-lg-4 py-3">
                    <button class="btn btn-light border d-lg-none" type="button" data-bs-toggle="offcanvas"
                            data-bs-target="#menu-lateral" aria-label="Abrir menú">
                        <i class="bi bi-list"></i>
                    </button>
                    <div class="me-auto">
                        <h1 class="h4 mb-0">{{ titulo }}</h1>
                        <p v-if="subtitulo" class="text-secondary small mb-0 d-none d-sm-block">{{ subtitulo }}</p>
                    </div>
                    <slot name="acciones"></slot>
                </header>
                <main class="px-3 px-lg-4 pb-5">
                    <slot></slot>
                </main>
            </div>
        </div>

        <div class="toast-container position-fixed bottom-0 end-0 p-3">
            <div v-for="aviso in avisos" :key="aviso.id" class="toast show border-0"
                 :class="aviso.tipo === 'error' ? 'text-bg-danger' : 'text-bg-success'" role="status">
                <div class="d-flex">
                    <div class="toast-body">
                        <i class="bi me-1" :class="aviso.tipo === 'error' ? 'bi-exclamation-triangle' : 'bi-check-circle'"></i>
                        {{ aviso.texto }}
                    </div>
                </div>
            </div>
        </div>
    `,
};

// ---------- Ventana emergente (modal) controlada por Vue ----------
const AppModal = {
    props: { abierto: Boolean, titulo: String, tamano: { type: String, default: '' } },
    emits: ['cerrar'],
    template: `
        <template v-if="abierto">
            <div class="modal d-block" tabindex="-1" @click.self="$emit('cerrar')">
                <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-fullscreen-sm-down"
                     :class="tamano ? 'modal-' + tamano : ''">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h2 class="modal-title h5">{{ titulo }}</h2>
                            <button type="button" class="btn-close" aria-label="Cerrar" @click="$emit('cerrar')"></button>
                        </div>
                        <div class="modal-body"><slot></slot></div>
                        <div v-if="$slots.pie" class="modal-footer"><slot name="pie"></slot></div>
                    </div>
                </div>
            </div>
            <div class="modal-backdrop show"></div>
        </template>
    `,
};

// ---------- Etiqueta de color con el estado del pedido ----------
const EstadoPedido = {
    props: { estado: String },
    computed: {
        info() { return ESTADOS_PEDIDO[this.estado] ?? { texto: this.estado, clase: 'text-bg-light' }; },
    },
    template: `<span class="badge rounded-pill" :class="info.clase">{{ info.texto }}</span>`,
};

// ---------- Indicador de carga y de error al traer datos ----------
const EstadoCarga = {
    props: { cargando: Boolean, error: String },
    emits: ['reintentar'],
    template: `
        <div v-if="cargando" class="text-center text-secondary py-5">
            <div class="spinner-border" role="status"></div>
            <p class="mt-2 mb-0">Cargando…</p>
        </div>
        <div v-else-if="error" class="alert alert-danger d-flex flex-wrap align-items-center gap-2">
            <i class="bi bi-wifi-off"></i>
            <span class="me-auto">{{ error }}</span>
            <button class="btn btn-sm btn-outline-danger" @click="$emit('reintentar')">Reintentar</button>
        </div>
    `,
};

function crearApp(opciones) {
    const app = Vue.createApp(opciones);
    app.component('app-layout', AppLayout);
    app.component('app-modal', AppModal);
    app.component('estado-pedido', EstadoPedido);
    app.component('estado-carga', EstadoCarga);
    app.config.globalProperties.$formato = formatos;
    return app;
}
