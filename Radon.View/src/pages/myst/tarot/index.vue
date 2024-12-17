<script lang="ts" setup>
import { useKeyUpdate } from "@/composable/useKeyUpdate.ts";
import { apiPost } from "@/lib/common/apiMethods";
import TarotDesc from "@/pages/myst/tarot/comps/TarotDesc.vue";
import TarotMain from "@/pages/myst/tarot/comps/TarotMain.vue";
import { type Card, mapCard } from "@/pages/myst/tarot/scripts/define";
import { useTarotStore } from "@/pages/myst/tarot/scripts/store";
import { get, set, useToggle } from "@vueuse/core";
import { useRouteHash } from "@vueuse/router";
import { storeToRefs } from "pinia";
import { CardIcon, RollbackIcon } from "tdesign-icons-vue-next";
import { MessagePlugin } from "tdesign-vue-next";
import { computed, onMounted } from "vue";
import { useI18n } from "vue-i18n";

const store = useTarotStore();
const { qDeck, qFull, qSize, activeTab, dShowDesc, dInfoMap, dInfoSelect, dInfoLoaded, results } = storeToRefs(store);
const { t } = useI18n();

const [loading, setLoading] = useToggle(false);
const { key, updateKey } = useKeyUpdate();
const hash = useRouteHash();

const deckFull = computed(() => get(dInfoMap)[qDeck.value]?.full ?? false);
const deckMax = computed(() => (get(deckFull) && get(qFull) ? 78 : 22));
const deckFullTooltip = computed(() => (get(deckFull) ? t("form.fullTT") : t("form.fullTT2")));
const resetAble = computed(() => get(qSize) > 1);
const resultExist = computed(() => results.value.length > 0);

const handleSelect = () => {
  if (!deckFull.value) set(qFull, false);
  set(results, []);
  updateKey();
};
const handleResetCount = () => set(qSize, 1);
const handleDraw = async () => {
  set(hash, "");
  setLoading(true);
  try {
    const f = (await apiPost<Card[]>("/api/tarot", store.query)).data.map(mapCard);
    set(results, f);
    updateKey();
  } catch (e) {
    console.error(e);
    void MessagePlugin.error(t("common.fetchError"));
  } finally {
    setLoading(false);
  }
};

onMounted(() => {
  if (!get(dInfoLoaded)) store.init();
});
</script>

<template>
  <content-layout :subtitle="t('st')" :title="t('tt')">
    <t-form @submit="handleDraw">
      <t-tabs v-model:value="activeTab">
        <t-tab-panel :label="t('tabs.simple')" value="simple">
          <div class="mt-2">
            <t-form-item :label="t('form.deck')">
              <sel-simple v-model="qDeck" :loading="!dInfoLoaded" :options="dInfoSelect" @update="handleSelect" />
            </t-form-item>
            <t-form-item :help="deckFullTooltip" :label="t('form.full')">
              <t-switch v-model="qFull" :disabled="!deckFull" />
            </t-form-item>
            <t-form-item :help="t('form.hintTT')" :label="t('form.hint')">
              <t-switch v-model="dShowDesc" />
            </t-form-item>
            <t-form-item :label="t('form.size')">
              <t-space :size="8">
                <t-input-number v-model="qSize" :max="deckMax" :min="1" />
                <t-button v-if="resetAble" shape="circle" theme="default" variant="outline" @click="handleResetCount">
                  <RollbackIcon />
                </t-button>
              </t-space>
            </t-form-item>
          </div>
        </t-tab-panel>
      </t-tabs>
      <div class="mt-4">
        <t-form-item :label="t('form.draw')">
          <t-button :loading="loading" shape="round" type="submit">
            <CardIcon v-if="!loading" />
          </t-button>
        </t-form-item>
      </div>
    </t-form>
    <!-- Result Area -->
    <div v-if="resultExist" :key="key">
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
form:
  deck: Deck
  full: Full Deck
  fullTT: "Full deck includes Minor Arcana"
  fullTT2: "This deck only includes Major Arcana"
  hintTT: "Show all the tips of the drawn cards below"
  hint: Hint
  size: Size
  draw: Draw
</i18n>

<i18n locale="zh-CN">
tt: "塔罗牌"
st: "模拟可能比自己切牌更顺畅"
tabs:
  simple: "简单操作"
form:
  deck: 卡组
  full: 完整卡组
  fullTT: "完整卡组包括小阿卡纳"
  fullTT2: "此卡组仅包括大阿卡纳"
  hintTT: "在下方展示所有抽到牌的提示"
  hint: 提示
  size: 数量
  draw: 抽牌
</i18n>
