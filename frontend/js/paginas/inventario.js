// Página de inventario: existencias de ingredientes y alertas de reabastecimiento (solo consulta)
crearApp({
    data: () => ({
        cargando: true,
        error: '',
        ingredientes: [],
        alertas: [],
        soloBajos: false,
    }),
    computed: {
        bajos() {
            return this.ingredientes.filter(i => i.bajoMinimo);
        },
        ingredientesVisibles() {
            return this.soloBajos ? this.bajos : this.ingredientes;
        },
    },
    methods: {
        async cargar() {
            this.cargando = true;
            this.error = '';
            try {
                [this.ingredientes, this.alertas] = await Promise.all([api.ingredientes.listar(), api.ingredientes.alertas()]);
            } catch (e) {
                this.error = e.message;
            } finally {
                this.cargando = false;
            }
        },
    },
    mounted() {
        this.cargar();
    },
}).mount('#app');
