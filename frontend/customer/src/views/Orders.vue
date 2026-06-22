<template>
  <div class="orders">
    <h2>我的订单</h2>
    <el-tabs v-model="activeTab">
      <el-tab-pane label="全部" name="all" />
      <el-tab-pane label="待确认" name="Pending" />
      <el-tab-pane label="已确认" name="Confirmed" />
      <el-tab-pane label="已完成" name="Completed" />
    </el-tabs>

    <el-table :data="filteredOrders" stripe>
      <el-table-column prop="services" label="服务项目" />
      <el-table-column prop="stylistName" label="发型师" />
      <el-table-column prop="appointmentDate" label="预约日期" :formatter="(r)=>r.appointmentDate?.slice(0,10)" />
      <el-table-column prop="timeSlot" label="时段" />
      <el-table-column prop="totalPrice" label="金额" />
      <el-table-column prop="status" label="状态">
        <template #default="{row}">
          <el-tag :type="statusType(row.status)">{{ statusText(row.status) }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="操作" width="180">
        <template #default="{row}">
          <el-button v-if="row.status==='Pending'" size="small" type="danger" @click="cancel(row.id)">取消</el-button>
          <el-button v-if="row.status==='Completed'" size="small" type="warning" @click="openReview(row)">评价</el-button>
        </template>
      </el-table-column>
    </el-table>

    <el-dialog v-model="reviewVisible" title="评价" width="400px">
      <el-rate v-model="reviewRating" />
      <el-input v-model="reviewContent" type="textarea" placeholder="写下你的评价..." style="margin-top:10px" />
      <template #footer>
        <el-button type="primary" @click="submitReview">提交评价</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from "vue";
import api from "../api";

const orders = ref([]);
const activeTab = ref("all");
const reviewVisible = ref(false);
const reviewRating = ref(5);
const reviewContent = ref("");
const currentApptId = ref(0);

const filteredOrders = computed(() => {
  if (activeTab.value === "all") return orders.value;
  return orders.value.filter(o => o.status === activeTab.value);
});

onMounted(async () => {
  const res = await api.get("/appointments/my");
  orders.value = res.data;
});

async function cancel(id) {
  await api.put(`/appointments/${id}/cancel`);
  const o = orders.value.find(x => x.id === id);
  if (o) o.status = "Cancelled";
}

function openReview(row) {
  currentApptId.value = row.id;
  reviewRating.value = 5;
  reviewContent.value = "";
  reviewVisible.value = true;
}

async function submitReview() {
  await api.post("/reviews", { appointmentId: currentApptId.value, rating: reviewRating.value, content: reviewContent.value });
  reviewVisible.value = false;
  alert("评价成功！");
}

function statusType(s) {
  return { Pending: "warning", Confirmed: "success", Completed: "info", Cancelled: "danger", Rejected: "danger" }[s] || "info";
}
function statusText(s) {
  return { Pending: "待确认", Confirmed: "已确认", Completed: "已完成", Cancelled: "已取消", Rejected: "已拒绝" }[s] || s;
}
</script>
