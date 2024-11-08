<script lang="ts" setup>
import TarotDesc from "@/pages/myst/tarot/comps/TarotDesc.vue";
import TarotMain from "@/pages/myst/tarot/comps/TarotMain.vue";
import { useTarotStore } from "@/pages/myst/tarot/scripts/store";
import type { Card } from "@/pages/myst/tarot/scripts/typeTarot";
import { CardIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";

const store = useTarotStore();
const { qDeck, qFull, qSize, activeTab, dShowDesc, dInfoMap, dInfoSelect, dInfoLoaded, results } = storeToRefs(store);
const { t } = useI18n();

const loading = ref(false);

const deckFull = computed(() => dInfoMap.value[qDeck.value]?.full ?? false);
const deckMax = computed(() => (deckFull.value && qFull.value ? 78 : 22));
const deckFullTooltip = computed(() => (deckFull.value ? "完整卡组包括小阿尔卡那" : "该卡组只包括大阿尔卡那"));

const handleSelect = () => {
  if (!deckFull.value) qFull.value = false;
  results.value.length = 0;
};
const handleDraw = async () => {
  location.hash = "";
  loading.value = true;
  try {
    results.value = (await apiPost<Card[]>("/api/tarot", store.query)).data.map(card => {
      return {
        data: card,
        showImg: true,
        showDesc: false,
      };
    });
  } catch (e) {
    console.error(e);
    await MessagePlugin.error(t("common.fetchError"));
  } finally {
    loading.value = false;
  }
};

onMounted(() => {
  store.init();
});
</script>

<template>
  <content-layout :title="t('tt')" :subtitle="t('st')">
    <t-form>
      <t-tabs v-model:value="activeTab">
        <t-tab-panel :label="t('tabs.simple')" value="simple">
          <div class="mt-2">
            <t-form-item label="卡面类型">
              <sel-simple v-model="qDeck" :loading="!dInfoLoaded" :options="dInfoSelect" @update="handleSelect" />
            </t-form-item>
            <t-form-item :help="deckFullTooltip" label="完整卡组">
              <t-switch v-model="qFull" :disabled="!deckFull" />
            </t-form-item>
            <t-form-item help="在下方展示所有抽到牌的提示" label="提示">
              <t-switch v-model="dShowDesc" />
            </t-form-item>
            <t-form-item label="数量">
              <t-input-number v-model="qSize" :max="deckMax" :min="1" />
            </t-form-item>
          </div>
        </t-tab-panel>
      </t-tabs>
      <div class="mt-4">
        <t-form-item label="抽牌">
          <t-loading :delay="90" :loading="loading" size="small">
            <t-button shape="round" @click="handleDraw">
              <CardIcon />
            </t-button>
          </t-loading>
        </t-form-item>
      </div>
    </t-form>
    <div v-if="results.length > 0">
      <t-divider />
      <t-row :gutter="[8, 8]">
        <t-col v-for="(card, index) in results" :key="index" :lg="3" :md="4" :sm="6" :xl="3" :xs="12" :xxl="3">
          <TarotMain :card="card" :index="index" />
        </t-col>
      </t-row>
      <div v-if="dShowDesc">
        <t-divider />
        <t-row :gutter="[10, 10]">
          <t-col v-for="(card, index) in results" :key="index" :lg="4" :md="4" :sm="6" :xl="4" :xs="12">
            <TarotDesc :card="card" :index="index" />
          </t-col>
        </t-row>
      </div>
    </div>
  </content-layout>
</template>

<i18n lang="yaml" locale="en">
tt: "Tarot"
st: "Simulation may be smoother than cutting your own cards"
tabs:
  simple: "Simple Operation"
</i18n>

<i18n locale="zh-CN">
tt: "塔罗牌"
st: "模拟可能比自己切牌更顺畅"
tabs:
  simple: "简单操作"
</i18n>
