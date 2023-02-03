const routes=[
    {path: '/home', component:home},
    {path: '/assignment', component:assignment},
    {path: '/course', component:course}
]

const router= VueRouter.createRouter({
    history: VueRouter.createWebHashHistory(),
    routes
})

const app = Vue.createApp({})
app.use(router)
app.mount('#app')
