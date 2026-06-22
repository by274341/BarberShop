<template>
  <div>
    <h2>我的业绩</h2>
    <el-row :gutter="20">
      <el-col :span="8"><el-card class="stat-card"><h3>今日接单</h3><p class="num">{{ stats.todayOrders }}</p></el-card></el-col>
      <el-col :span="8"><el-card class="stat-card"><h3>本周接单</h3><p class="num">{{ stats.weekOrders }}</p></el-card></el-col>
      <el-col :span="8"><el-card class="stat-card"><h3>本月收入</h3><p class="num">¥{{ stats.monthRevenue }}</p></el-card></el-col>
    </el-row>
    <h3 style="margin-top:20px">近7天趋势</h3>
    <div ref="chartRef" style="width:100%;height:300px"></div>
  </div>
</template>

<script setup>
import { ref, onMounted, nextTick } from "vue";
import api from "../api";
import * as echarts from "echarts";

const stats = ref({ todayOrders: 0, weekOrders: 0, monthRevenue: 0, weeklyTrend: [] });
const chartRef = ref(null);

onMounted(async () => {
  const res = await api.get("/stylist/stats");
  stats.value = res.data;
  await nextTick();
  if (chartRef.value && stats.value.weeklyTrend.length > 0) {
    const chart = echarts.init(chartRef.value);
    chart.setOption({
      tooltip: { trigger: "axis" },
      xAxis: { type: "category", data: stats.value.weeklyTrend.map(d => d.date) },
      yAxis: { type: "value" },
      series: [{ data: stats.value.weeklyTrend.map(d => d.count), type: "line", smooth: true }]
    });
  }
});
</script>

<style scoped>
.stat-card { text-align: center; }
.num { font-size: 32px; font-weight: bold; color: #409eff; margin-top: 10px; }
</style>
