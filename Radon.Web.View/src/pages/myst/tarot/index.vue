<script lang="ts" setup>
import SelectSimple from "@/components/select/SelSimple.vue";
import ContentLayout from "@/layout/frame/ContentLayout.vue";
import type { Card, CardDisplay, DeckInfo, DeckInfoInterface, DeckInfoSelect } from "@/lib/type/typeTarot";
import { apiGet, apiPost } from "@/lib/util/apiMethods";
import TarotDesc from "@/pages/myst/tarot/comps/TarotDesc.vue";
import TarotMain from "@/pages/myst/tarot/comps/TarotMain.vue";
import { useTarotStore } from "@/pages/myst/tarot/scripts/store";
import { CardIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";

const store = useTarotStore();

const query = reactive({
  deck: "waite",
  full: true,
  size: 1,
});
const conf = reactive({
  activeTab: "simple",
  deckShowDesc: true,
});
const loading = ref(false);

const deckInfoMap: Ref<DeckInfoInterface> = ref({});
const deckInfoList: Ref<DeckInfo[]> = ref([]);
const deckInfoSelect: Ref<DeckInfoSelect[]> = ref([]);
const selectLoading = ref(true);

apiGet("/api/tarot/info")
  .then(res => {
    const response = res.data as DeckInfoInterface;
    deckInfoMap.value = response;
    const list = Object.values(response) as DeckInfo[];
    deckInfoList.value = list;
    deckInfoSelect.value = list.map(item => {
      return {
        value: item.name,
        label: item.loc,
      };
    });
  })
  .then(() => (selectLoading.value = false));

const deckFull = computed(() => deckInfoMap.value[query.deck]?.full ?? false);
const deckMax = computed(() => (deckFull.value && query.full ? 78 : 22));
const deckFullTooltip = computed(() => (deckFull.value ? "完整卡组包括小阿尔卡那" : "该卡组只包括大阿尔卡那"));

const handleSelect = () => {
  if (!deckFull.value) query.full = false;
  result.value.length = 0;
};
const handleDraw = async () => {
  location.hash = "";
  loading.value = true;
  try {
    store.update(query, conf);
    const respond = (await apiPost("/api/tarot", query)).data as Card[];
    result.value = respond.map(card => {
      return {
        data: card,
        showImg: true,
        showDesc: false,
      };
    });
  } catch (e) {
    console.error(e);
    await MessagePlugin.error("与服务器通信失败");
  } finally {
    loading.value = false;
  }
};

const result = ref<CardDisplay[]>([]);

onMounted(() => {
  query.deck = store.deck;
  query.full = store.full;
  query.size = store.size;
});
</script>

<template>
  <ContentLayout title="塔罗" subtitle="模拟的说不定比你自己切的牌都顺">
    <t-form>
      <t-tabs v-model:value="conf.activeTab">
        <t-tab-panel label="简单操作" value="simple">
          <div class="mt-2">
            <t-form-item label="卡面类型">
              <SelectSimple
                v-model:selected="query.deck"
                :loading="selectLoading"
                :options="deckInfoSelect"
                @update="handleSelect"
              />
            </t-form-item>
            <t-form-item :help="deckFullTooltip" label="完整卡组">
              <t-switch v-model="query.full" :disabled="!deckFull" />
            </t-form-item>
            <t-form-item help="在下方展示所有抽到牌的提示" label="提示">
              <t-switch v-model="conf.deckShowDesc" />
            </t-form-item>
            <t-form-item label="数量">
              <t-input-number v-model="query.size" :max="deckMax" :min="1" />
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
    <div v-if="result.length > 0">
      <t-divider />
      <t-row :gutter="[8, 8]">
        <t-col v-for="(card, index) in result" :key="index" :lg="3" :md="4" :sm="6" :xl="3" :xs="12" :xxl="3">
          <TarotMain :card="card" :index="index" />
        </t-col>
      </t-row>
      <div v-if="conf.deckShowDesc">
        <t-divider />
        <t-row :gutter="[10, 10]">
          <t-col v-for="(card, index) in result" :key="index" :lg="4" :md="4" :sm="6" :xl="4" :xs="12">
            <TarotDesc :card="card" :index="index" />
          </t-col>
        </t-row>
      </div>
    </div>
  </ContentLayout>
</template>
