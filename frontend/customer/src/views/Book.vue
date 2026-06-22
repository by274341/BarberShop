<template>
  <div class="book">
    <el-steps :active="step" align-center style="margin-bottom:30px">
      <el-step title="选项目" /><el-step title="选发型师" /><el-step title="选时间" /><el-step title="确认订单" />
    </el-steps>

    <!-- Step 1: 选项目 -->
    <div v-if="step===0">
      <h3>选择服务项目</h3>
      <el-checkbox-group v-model="selectedServices">
        <el-row :gutter="15">
          <el-col :span="8" v-for="s in services" :key="s.id">
            <el-card class="svc-card" :class="{ selected: selectedServices.includes(s.id) }">
              <el-checkbox :value="s.id" :label="s.id">
                <b>{{ s.name }}</b>
              </el-checkbox>
              <p class="price">¥{{ s.price }}</p>
              <p class="cat">{{ s.category }} · {{ s.duration }}分钟</p>
            </el-card>
          </el-col>
        </el-row>
      </el-checkbox-group>
      <p class="total">合计：¥{{ totalPrice }}</p>
      <el-button type="primary" @click="step=1" :disabled="selectedServices.length===0">下一步</el-button>
    </div>

    <!-- Step 2: 选发型师 -->
    <div v-if="step===1">
      <h3>选择发型师</h3>
      <el-radio-group v-model="selectedStylist">
        <el-radio :value="0">不指定（系统分配）</el-radio>
        <el-row :gutter="15" style="margin-top:10px">
          <el-col :span="8" v-for="s in stylists" :key="s.id">
            <el-card class="st-card" :class="{ selected: selectedStylist===s.userId }">
              <el-radio :value="s.userId">
                <b>{{ s.nickname }}</b> · {{ s.level }}
              </el-radio>
              <p>{{ s.specialty }}</p>
            </el-card>
          </el-col>
        </el-row>
      </el-radio-group>
      <div style="margin-top:20px">
        <el-button @click="step=0">上一步</el-button>
        <el-button type="primary" @click="step=2">下一步</el-button>
      </div>
    </div>

    <!-- Step 3: 选时间 -->
    <div v-if="step===2">
      <h3>选择预约日期</h3>
      <el-date-picker v-model="appointmentDate" type="date" placeholder="选择日期" :disabled-date="disablePast" />
      <h3 style="margin-top:20px">选择时段</h3>
      <el-radio-group v-model="timeSlot">
        <el-radio v-for="t in timeSlots" :key="t" :value="t" style="margin:5px">{{ t }}</el-radio>
      </el-radio-group>
      <div style="margin-top:20px">
        <el-button @click="step=1">上一步</el-button>
        <el-button type="primary" @click="step=3" :disabled="!appointmentDate||!timeSlot">下一步</el-button>
      </div>
    </div>

    <!-- Step 4: 确认 -->
    <div v-if="step===3">
      <h3>确认订单</h3>
      <el-descriptions border :column="1">
        <el-descriptions-item label="服务项目">{{ selectedServices.map(id=>services.find(s=>s.id===id)?.name).join("、") }}</el-descriptions-item>
        <el-descriptions-item label="发型师">{{ selectedStylist===0 ? "不指定" : stylists.find(s=>s.userId===selectedStylist)?.nickname }}</el-descriptions-item>
        <el-descriptions-item label="预约时间">{{ appointmentDate }} {{ timeSlot }}</el-descriptions-item>
        <el-descriptions-item label="金额">¥{{ totalPrice }}</el-descriptions-item>
      </el-descriptions>
      <div style="margin-top:20px">
        <el-button @click="step=2">上一步</el-button>
        <el-button type="primary" @click="submit" :loading="submitting">提交预约</el-button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";
import api from "../api";

const route = useRoute();
const router = useRouter();
const step = ref(0);
const services = ref([]);
const stylists = ref([]);
const promotions = ref([]);
const selectedServices = ref([]);
const selectedStylist = ref(0);
const appointmentDate = ref("");
const timeSlot = ref("");
const submitting = ref(false);

const timeSlots = ["09:00-09:30","09:30-10:00","10:00-10:30","10:30-11:00","11:00-11:30","11:30-12:00","13:00-13:30","13:30-14:00","14:00-14:30","14:30-15:00","15:00-15:30","15:30-16:00","16:00-16:30","16:30-17:00","17:00-17:30","17:30-18:00"];

const totalPrice = computed(() => selectedServices.value.reduce((sum, id) => {
  const s = services.value.find(x => x.id === id);
  if (!s) return sum;
  let price = s.price;
  const promo = promotions.value.find(p => p.serviceId === id);
  if (promo) price = Math.round(price * promo.discountRate * 100) / 100;
  return sum + price;
}, 0));

onMounted(async () => {
  const [sRes, stRes, pRes] = await Promise.all([api.get("/services"), api.get("/stylists"), api.get("/promotions/active")]);
  services.value = sRes.data;
  stylists.value = stRes.data;
  promotions.value = pRes.data;
  if (route.query.stylistId) selectedStylist.value = parseInt(route.query.stylistId);
});

function disablePast(date) { return date.getTime() < Date.now() - 86400000; }

async function submit() {
  submitting.value = true;
  try {
    await api.post("/appointments", {
      serviceIds: selectedServices.value,
      stylistId: selectedStylist.value === 0 ? null : selectedStylist.value,
      appointmentDate: appointmentDate.value,
      timeSlot: timeSlot.value,
      note: null
    });
    alert("预约成功！");
    router.push("/orders");
  } catch (e) {
    alert(e.response?.data?.message || "预约失败");
  } finally {
    submitting.value = false;
  }
}
</script>

<style scoped>
.book { max-width: 900px; margin: 0 auto; }
.svc-card, .st-card { margin-bottom: 10px; cursor: pointer; }
.svc-card.selected, .st-card.selected { border-color: #409eff; background: #ecf5ff; }
.price { color: #e6a23c; font-weight: bold; }
.cat { color: #999; font-size: 13px; }
.total { font-size: 20px; color: #e6a23c; margin: 15px 0; }
</style>
