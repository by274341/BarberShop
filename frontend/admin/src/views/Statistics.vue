<template>
  <div>
    <h2>数据统计</h2>
    <el-form inline>
      <el-form-item><el-date-picker v-model="dateRange" type="daterange" range-separator="至" @change="loadRevenue" /></el-form-item>
      <el-form-item>
        <el-radio-group v-model="chartType" @change="loadRevenue">
          <el-radio-button value="line">折线图</el-radio-button>
          <el-radio-button value="bar">柱状图</el-radio-button>
        </el-radio-group>
      </el-form-item>
    </el-form>
    <el-card>
      <h3>营收统计</h3>
      <div ref="revenueChart" style="height:350px"></div>
    </el-card>
    <el-card style="margin-top:20px">
      <h3>服务销量占比</h3>
      <div ref="pieChart" style="height:350px"></div>
    </el-card>
  </div>
</template>

<script setup>
import { ref, onMounted, nextTick } from "vue";
import api from "../api";
import * as echarts from "echarts";

const dateRange = ref([new Date(Date.now()-30*86400000), new Date()]);
const chartType = ref("line");
const revenueChart = ref(null);
const pieChart = ref(null);
let revenueChartInst = null;
let pieChartInst = null;

onMounted(async () => {
  await loadRevenue();
  await loadPie();
});

async function loadRevenue() {
  const [start, end] = dateRange.value;
  const res = await api.get("/dashboard/revenue", {
    params: { start: start.toISOString().slice(0,10), end: end.toISOString().slice(0,10) }
  });
  const data = res.data;
  await nextTick();
  if (revenueChart.value) {
    if (!revenueChartInst) revenueChartInst = echarts.init(revenueChart.value);
    revenueChartInst.setOption({
      tooltip: { trigger: "axis" },
      xAxis: { type: "category", data: data.map(d => d.date) },
      yAxis: { type: "value", name: "营收(元)" },
      series: [{
        data: data.map(d => d.revenue),
        type: chartType.value,
        smooth: true,
        areaStyle: chartType.value==='line' ? {} : undefined
      }]
    });
  }
}

async function loadPie() {
  const res = await api.get("/dashboard/service-stats");
  await nextTick();
  if (pieChart.value) {
    if (!pieChartInst) pieChartInst = echarts.init(pieChart.value);
    pieChartInst.setOption({
      tooltip: { trigger: "item" },
      series: [{
        type: "pie",
        radius: ["40%","70%"],
        data: res.data
      }]
    });
  }
}
</script>
