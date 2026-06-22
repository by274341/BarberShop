<template>
  <div>
    <h2>我的排班</h2>
    <p class="tip">设置未来一周每天的上班时段，顾客端将显示对应时段</p>
    <el-card v-for="(day, idx) in schedule" :key="idx" style="margin-bottom:10px">
      <el-row align="middle">
        <el-col :span="4"><b>{{ weekNames[day.dayOfWeek] }}</b></el-col>
        <el-col :span="8">
          <el-switch v-model="day.isOff" active-text="休息" inactive-text="上班" @change="onToggle(day)" />
        </el-col>
        <el-col :span="12" v-if="!day.isOff">
          <el-time-select v-model="day.startTime" start="09:00" end="18:00" step="00:30" placeholder="开始" />
          至
          <el-time-select v-model="day.endTime" start="09:00" end="18:00" step="00:30" placeholder="结束" />
        </el-col>
      </el-row>
    </el-card>
    <el-button type="primary" @click="save" :loading="saving">保存排班</el-button>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted } from "vue";
import api from "../api";

const weekNames = ["周日","周一","周二","周三","周四","周五","周六"];
const schedule = reactive([
  { dayOfWeek:0, startTime:"09:00", endTime:"18:00", isOff:true },
  { dayOfWeek:1, startTime:"09:00", endTime:"18:00", isOff:false },
  { dayOfWeek:2, startTime:"09:00", endTime:"18:00", isOff:false },
  { dayOfWeek:3, startTime:"09:00", endTime:"18:00", isOff:false },
  { dayOfWeek:4, startTime:"09:00", endTime:"18:00", isOff:false },
  { dayOfWeek:5, startTime:"09:00", endTime:"18:00", isOff:false },
  { dayOfWeek:6, startTime:"09:00", endTime:"18:00", isOff:true },
]);
const saving = ref(false);

onMounted(async () => {
  try {
    const res = await api.get("/stylist/schedule");
    if (res.data.length > 0) {
      res.data.forEach(item => {
        const day = schedule.find(d => d.dayOfWeek === item.dayOfWeek);
        if (day) {
          day.startTime = item.startTime;
          day.endTime = item.endTime;
          day.isOff = item.isOff;
        }
      });
    }
  } catch {}
});

function onToggle(day) { if (day.isOff) { day.startTime = "09:00"; day.endTime = "18:00"; } }

async function save() {
  saving.value = true;
  try {
    await api.put("/stylist/schedule", {
      items: schedule.map(d => ({ dayOfWeek: d.dayOfWeek, startTime: d.startTime, endTime: d.endTime, isOff: d.isOff }))
    });
    alert("排班保存成功！");
  } catch (e) {
    alert("保存失败");
  } finally {
    saving.value = false;
  }
}
</script>

<style scoped>
.tip { color: #999; margin-bottom: 15px; }
</style>