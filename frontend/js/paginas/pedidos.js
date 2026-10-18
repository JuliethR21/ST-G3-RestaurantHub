// Página de pedidos: historial con filtros, detalle y cambio de estado

// Siguiente paso de cada estado (igual a las transiciones que permite el backend)
const SIGUIENTE_ESTADO = {
    PENDIENTE: { estado: 'PREPARANDO', texto: 'Pasar a preparación', icono: 'bi-fire' },
    PREPARANDO: { estado: 'ENTREGADO', texto: 'Marcar como entregado', icono: 'bi-check2-circle' },
};

crearApp({
    data: () => ({
        cargando: true,
        error: '',
        pedidos: [],
        estados: ESTADOS_PEDIDO,
        filtros: { estado: '', desde: '', hasta: '' },
        detalle: { abierto: false, id: null, cargando: false, error: '', guardando: false, pedido: null },
    }),
    computed: {
        siguienteEstado() {
            return SIGUIENTE_ESTADO[this.detalle.pedido?.estado] ?? null;
        },
        puedeCancelar() {
            const p = this.detalle.pedido;
            return p && !p.pagado && (p.estado === 'PENDIENTE' || p.estado === 'PREPARANDO');
        },
    },
    methods: {
        async cargar() {
            this.cargando = true;
            this.error = '';
            try {
                this.pedidos = await api.pedidos.listar(this.filtros);
            } catch (e) {
                this.error = e.message;
            } finally {
                this.cargando = false;
            }
        },
        limpiarFiltros() {
            this.filtros = { estado: '', desde: '', hasta: '' };
            this.cargar();
        },
        async abrirDetalle(id) {
            this.detalle = { abierto: true, id, cargando: true, error: '', guardando: false, pedido: null };
            try {
                this.detalle.pedido = await api.pedidos.obtener(id);
            } catch (e) {
                this.detalle.error = e.message;
            } finally {
                this.detalle.cargando = false;
            }
        },
        cerrarDetalle() {
            this.detalle.abierto = false;
        },
        async cambiarEstado(nuevoEstado) {
            if (nuevoEstado === 'CANCELADO' &&
                !confirm('¿Cancelar este pedido? Los ingredientes volverán al inventario.')) return;

            this.detalle.guardando = true;
            try {
                const pedido = await api.pedidos.cambiarEstado(this.detalle.pedido.idPedido, nuevoEstado);
                this.detalle.pedido = pedido;
                const fila = this.pedidos.find(p => p.idPedido === pedido.idPedido);
                if (fila) fila.estado = pedido.estado;
                notificar('exito', `Pedido #${pedido.idPedido}: ${ESTADOS_PEDIDO[pedido.estado].texto}.`);
            } catch (e) {
                notificar('error', e.message);
            } finally {
                this.detalle.guardando = false;
            }
        },
    },
    mounted() {
        // Permite llegar con filtros o abrir un pedido desde otras páginas: pedidos.html?estado=PENDIENTE o ?id=5
        const parametros = new URLSearchParams(window.location.search);
        if (parametros.has('estado')) this.filtros.estado = parametros.get('estado');
        this.cargar();
        if (parametros.has('id')) this.abrirDetalle(Number(parametros.get('id')));
    },
}).mount('#app');
