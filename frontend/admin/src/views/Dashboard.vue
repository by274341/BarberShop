<template>
  <div>
    <h2>工作台</h2>
    <el-row :gutter="20">
      <el-col :span="6"><el-card class="card"><h3>今日预约</h3><p class="num">{{ dash.todayAppointments }}</p></el-card></el-col>
      <el-col :span="6"><el-card class="card"><h3>今日营收</h3><p class="num">¥{{ dash.todayRevenue }}</p></el-card></el-col>
      <el-col :span="6"><el-card class="card"><h3>在岗发型师</h3><p class="num">{{ dash.activeStylists }}</p></el-card></el-col>
      <el-col :span="6"><el-card class="card"><h3>待确认订单</h3><p class="num">{{ dash.pendingOrders }}</p></el-card></el-col>
    </el-row>

    <el-row :gutter="20" style="margin-top:20px">
      <el-col :span="12">
        <el-card><h3>近7天营收/预约趋势</h3><div ref="trendChart" style="height:300px"></div></el-card>
      </el-col>
      <el-col :span="12">
        <el-card><h3>发型师业绩排行</h3>
          <el-table :data="performance" stripe size="small">
            <el-table-column prop="nickname" label="姓名" />
            <el-table-column prop="totalOrders" label="接单数" />
            <el-table-column prop="totalRevenue" label="营收" />
          </el-table>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script setup>
import { ref, onMounted, onUnmounted, nextTick } from "vue";
import api from "../api";
import * as echarts from "echarts";

const dash = ref({ todayAppointments: 0, todayRevenue: 0, activeStylists: 0, pendingOrders: 0, weeklyStats: [] });
const performance = ref([]);
const trendChart = ref(null);
let trendChartInst = null;

onMounted(async () => {
  const [dRes, pRes] = await Promise.all([
    api.get("/dashboard"),
    api.get("/dashboard/performance")
  ]);
  dash.value = dRes.data;
  performance.value = pRes.data;

  await nextTick();
  if (trendChart.value && dash.value.weeklyStats.length > 0) {
    trendChartInst = echarts.init(trendChart.value);
    trendChartInst.setOption({
      tooltip: { trigger: "axis" },
      legend: { data: ["预约量","营收"] },
      xAxis: { type: "category", data: dash.value.weeklyStats.map(d => d.date) },
      yAxis: [{ type: "value", name: "预约量" }, { type: "value", name: "营收(元)" }],
      series: [
        { name: "预约量", data: dash.value.weeklyStats.map(d => d.count), type: "bar" },
        { name: "营收", data: dash.value.weeklyStats.map(d => d.revenue), type: "line", yAxisIndex: 1, smooth: true }
      ]
    });
  }
  window.addEventListener('resize', () => trendChartInst?.resize());
});

onUnmounted(() => {
  window.removeEventListener('resize', () => trendChartInst?.resize());
  trendChartInst?.dispose();
});
</script>

<style scoped>
.card { text-align: center; }
.num { font-size: 36px; font-weight: bold; color: #409eff; margin-top: 10px; }
</style>
