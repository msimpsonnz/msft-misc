<template>
    <div id="shop">
        <h2>{{ shop }}</h2>
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-3">
                    <h3>Category</h3>
                    <ul>
                        <li v-for="product in productCategories" :key="product.productId">
                            <input type="checkbox" v-model="checkedCategory" v-bind:value="product.category" />  {{product.category}}
                        </li>
                    </ul>
                    <span>Checked category: {{ checkedCategory }}</span>
                </div>
                <div class="col-md-9">
                    <h3>Products</h3>
                    <div class="products">
                        <div class="products" v-for="product in filteredProducts" :key="product.productId">
                            <div class="card" style="margin: 5px 0">
                                <div class="card-block">
                                    <h4 class="card-title">
                                        {{ product.description }}
                                    </h4>
                                    <small class="pull-right text-muted" style="font-size: 12x">
                                        {{ product.category }}
                                    </small>
                                <div>
                                    <!-- <div v-if="product.segment === 9901"> -->
                                        <button class="btn btn-sm btn-primary" v-on:click="sendData(product)">Buy Now!</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import axios from 'axios';
    import uniq from 'lodash';
    import modal from './modal.vue';
    export default {
    name: 'shop',
    components: {
      modal
    },
    data() {
    return {
    isModalVisible: false,
    shop: "Bob's Awesome Appliances!",
    products: [],
    checkedCategory: [],
    }
    },
    created(){
    this.getData();
    },
    computed: {
        filteredProducts: function filteredProducts() {
        var _this = this;

        if (!this.checkedCategory.length) return this.products;
        return this.products.filter(function (j) {
        return _this.checkedCategory.includes(j.category);
        });
        },

        productCategories: function productCategories() {
        return _.uniqBy(this.products, 'category')
        }
    },
    methods: {
    sendData: function(input) {
    //console.log("ViewButtonClick", { productId: input.productId, productDescription: input.description });

    // example of custom event code
    appInsights.trackEvent("ViewButtonClick", { productId: input.productId, productDescription: input.description });

    axios({ method: "POST", "url": "/api/shop", "data": input, "headers": { "content-type": "application/json" } }).then(result => {
    this.response = result.data;
    }, error => {
    console.error(error);
    });
    },

    getData() {
    axios({ method: "GET", "url": "/api/shop", responseType: 'json' })
    .then(result => {
    this.products = result.data;
    });
    },
          showModal() {
        this.isModalVisible = true;
      },
      closeModal() {
        this.isModalVisible = false;
      }
    }
    };
</script>

<style>
</style>
