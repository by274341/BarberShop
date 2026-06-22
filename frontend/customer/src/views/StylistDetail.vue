<template>
  <div class="detail" v-if="stylist">
    <el-row :gutter="20">
      <el-col :span="8">
        <el-card>
          <div class="avatar">{{ stylist.nickname[0] }}</div>
          <h2>{{ stylist.nickname }}</h2>
          <p>等级：{{ stylist.level }}</p>
          <p>从业：{{ stylist.years }}年</p>
          <p>擅长：{{ stylist.specialty }}</p>
          <p class="desc">{{ stylist.description }}</p>
          <el-rate :model-value="stylist.avgRating" disabled show-score />
          <p>共 {{ stylist.reviewCount }} 条评价</p>
          <el-button type="primary" size="large" style="margin-top:20px;width:100%" @click="goBook">立即预约</el-button>
          <el-button :type="isFav ? 'warning' : 'default'" size="large" style="margin-top:10px;width:100%" @click="toggleFav" :loading="favLoading">{{ isFav ? '取消收藏' : '收藏' }}</el-button>
        </el-card>
      </el-col>
      <el-col :span="16">
        <h3>顾客评价</h3>
        <div v-for="r in reviews" :key="r.id" class="review-item">
          <div class="review-header">
            <span class="reviewer">{{ r.customerName }}</span>
            <el-rate :model-value="r.rating" disabled size="small" />
            <span class="time">{{ r.createdAt?.slice(0,10) }}</span>
          </div>
          <p>{{ r.content }}</p>
        </div>
        <el-empty v-if="reviews.length===0" description="暂无评价" />
      </el-col>
    </el-row>
  </div>
  <div v-else-if="error" class="error-box">
    <el-result icon="error" title="加载失败" sub-title="请先登录后再查看">
      <template #extra><el-button type="primary" @click="$router.push('/login')">去登录</el-button></template>
    </el-result>
  </div>
  <div v-else style="text-align:center;padding:100px">加载中...</div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";
import api from "../api";

const route = useRoute();
const router = useRouter();
const stylist = ref(null);
const reviews = ref([]);
const error = ref(false);
const isFav = ref(false);
const favLoading = ref(false);

onMounted(async () => {
  try {
    const id = route.params.id;
    const [sRes, rRes] = await Promise.all([
      api.get(`/stylists/${id}`),
      api.get(`/stylists/${id}/reviews`)
    ]);
    stylist.value = sRes.data;
    reviews.value = rRes.data;
    try { const fRes = await api.get("/favorites"); isFav.value = fRes.data.some(f => f.id === parseInt(id)); } catch {}
  } catch { error.value = true; }
});

async function toggleFav() {
  favLoading.value = true;
  try {
    if (isFav.value) {
      await api.delete(`/favorites/${route.params.id}`);
      isFav.value = false;
    } else {
      await api.post(`/favorites/${route.params.id}`);
      isFav.value = true;
    }
  } catch {} finally { favLoading.value = false; }
}

function goBook() {
  router.push({ path: "/book", query: { stylistId: stylist.value.userId } });
}
</script>

<style scoped>
.detail { padding: 20px; }
.avatar { width: 80px; height: 80px; border-radius: 50%; background: #409eff; color: #fff; font-size: 32px; line-height: 80px; text-align: center; margin: 0 auto 15px; }
.desc { color: #666; margin: 10px 0; }
.review-item { border-bottom: 1px solid #eee; padding: 15px 0; }
.review-header { display: flex; align-items: center; gap: 10px; }
.reviewer { font-weight: bold; }
.time { color: #999; font-size: 13px; }
.error-box { text-align: center; padding: 100px 0; }
</style>