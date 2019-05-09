import Shop from 'components/shop'
import HomePage from 'components/home-page'

export const routes = [
    { path: '/', component: HomePage, display: 'Home', style: 'glyphicon glyphicon-home' },
    { path: '/shop', component: Shop, display: 'Shop', style: 'glyphicon glyphicon-shopping-cart' }
]
