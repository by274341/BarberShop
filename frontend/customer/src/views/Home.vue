<template>
  <div class="home">
    <el-carousel height="300px" class="banner">
      <el-carousel-item v-for="i in 3" :key="i">
        <div class="banner-item" :style="{ background: ['#409eff','#67c23a','#e6a23c'][i-1] }">
          <h1>欢迎光临理发店</h1><p>专业造型，精致生活</p>
        </div>
      </el-carousel-item>
    </el-carousel>

    <h2 class="section-title">服务项目</h2>
    <el-row :gutter="20">
      <el-col :span="6" v-for="s in services" :key="s.id">
        <el-card class="service-card" shadow="hover">
          <h3>{{ s.name }}</h3>
          <p class="category">{{ s.category }}</p>
          <p class="price">¥{{ s.price }} / {{ s.duration }}分钟</p>
          <p class="desc">{{ s.description }}</p>
        </el-card>
      </el-col>
    </el-row>

    <h2 class="section-title">推荐发型师</h2>
    <el-row :gutter="20">
      <el-col :span="6" v-for="s in stylists" :key="s.id">
        <el-card class="stylist-card" shadow="hover" @click="$router.push(`/stylist/${s.id}`)">
          <div class="avatar">{{ s.nickname[0] }}</div>
          <h3>{{ s.nickname }}</h3>
          <p>{{ s.level }} · {{ s.years }}年</p>
          <p class="specialty">{{ s.specialty }}</p>
          <el-rate :model-value="s.avgRating" disabled show-score />
          <el-button type="primary" size="small" style="margin-top:10px">查看详情</el-button>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import api from "../api";

const services = ref([]);
const stylists = ref([]);

onMounted(async () => {
  const [sRes, stRes] = await Promise.all([
    api.get("/services"),
    api.get("/stylists")
  ]);
  services.value = sRes.data;
  stylists.value = stRes.data;
});
</script>

<style scoped>
.banner-item { height: 100%; display: flex; flex-direction: column; justify-content: center; align-items: center; color: #fff; }
.banner-item h1 { font-size: 36px; }
.section-title { margin: 30px 0 15px; padding-left: 10px; border-left: 4px solid #409eff; }
.service-card { margin-bottom: 15px; text-align: center; }
.service-card .price { color: #e6a23c; font-weight: bold; font-size: 18px; }
.service-card .category { color: #909399; }
.service-card .desc { color: #666; font-size: 13px; margin-top: 5px; }
.stylist-card { margin-bottom: 15px; text-align: center; cursor: pointer; }
.avatar { width: 60px; height: 60px; border-radius: 50%; background: #409eff; color: #fff; font-size: 24px; line-height: 60px; margin: 0 auto 10px; }
.specialty { color: #67c23a; font-size: 13px; }
</style>
