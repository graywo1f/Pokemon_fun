import Vue from 'vue';
import HomeComponent from './home.vue';
import DetailsComponent from './details.vue';

new Vue({
    el: "#app",
    components: {
        HomeComponent,
        DetailsComponent
       }
})