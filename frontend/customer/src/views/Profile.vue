<template>
  <div class="profile">
    <h2>个人中心</h2>
    <el-card>
      <el-descriptions :column="1" border>
        <el-descriptions-item label="昵称">{{ user?.nickname }}</el-descriptions-item>
        <el-descriptions-item label="用户名">{{ user?.username }}</el-descriptions-item>
        <el-descriptions-item label="手机号">{{ user?.phone }}</el-descriptions-item>
      </el-descriptions>
    </el-card>
    <el-card style="margin-top:20px">
      <h3>我的收藏（发型师）</h3>
      <el-row :gutter="15" v-if="favorites.length">
        <el-col :span="8" v-for="s in favorites" :key="s.id">
          <el-card class="fav-card" shadow="hover" @click="$router.push(`/stylist/${s.id}`)">
            <div class="fav-avatar">{{ s.nickname[0] }}</div>
            <p><b>{{ s.nickname }}</b></p>
            <p>{{ s.level }} · {{ s.years }}年</p>
            <el-rate :model-value="s.avgRating" disabled show-score size="small" />
            <el-button size="small" type="danger" style="margin-top:5px" @click.stop="removeFav(s.id)">取消收藏</el-button>
          </el-card>
        </el-col>
      </el-row>
      <el-empty v-else description="暂无收藏" />
    </el-card>
    <el-card style="margin-top:20px">
      <h3>优惠券</h3>
      <el-row :gutter="15" v-if="coupons.length">
        <el-col :span="8" v-for="c in coupons" :key="c.id">
          <el-card class="coupon-card" :class="{ used: c.isUsed }">
            <div class="coupon-value">&yen;{{ c.faceValue }}</div>
            <p>{{ c.name }}</p>
            <p class="coupon-info">满{{ c.minAmount }}可用</p>
            <p class="coupon-info">有效期至 {{ c.validTo?.slice(0,10) }}</p>
            <el-tag v-if="c.isUsed" type="info" size="small">已使用</el-tag>
            <el-tag v-else type="success" size="small">可用</el-tag>
          </el-card>
        </el-col>
      </el-row>
      <el-empty v-else description="暂无优惠券" />
    </el-card>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import api from "../api";

const user = ref(null);
const favorites = ref([]);
const coupons = ref([]);

onMounted(async () => {
  try {
    const [uRes, fRes, cRes] = await Promise.all([
      api.get("/auth/me"),
      api.get("/favorites"),
      api.get("/coupons/my")
    ]);
    user.value = uRes.data;
    favorites.value = fRes.data;
    coupons.value = cRes.data;
  } catch { /* ignore */ }
});

async function removeFav(stylistId) {
  await api.delete(`/favorites/${stylistId}`);
  favorites.value = favorites.value.filter(f => f.id !== stylistId);
}
</script>

<style scoped>
.fav-card { text-align: center; cursor: pointer; margin-bottom: 10px; }
.fav-avatar { width: 50px; height: 50px; border-radius: 50%; background: #409eff; color: #fff; font-size: 20px; line-height: 50px; margin: 0 auto 8px; }
.coupon-card { text-align: center; margin-bottom: 10px; border: 1px solid #e6a23c; }
.coupon-card.used { opacity: 0.5; border-color: #ccc; }
.coupon-value { font-size: 28px; color: #e6a23c; font-weight: bold; }
.coupon-info { color: #999; font-size: 13px; }
</style>